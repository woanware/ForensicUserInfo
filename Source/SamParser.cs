using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace woanware
{
    public class SamParser
    {
        //private static Hashtable _acbs = new Hashtable()
        //{
        //    {0x0001, "Account Disabled"},
        //    {0x0002, "Home directory required"},
        //    {0x0004, "Password not required"},
        //    {0x0008, "Temporary duplicate account"},
        //    {0x0010, "Normal user account"},
        //    {0x0020, "MNS logon user account"},
        //    {0x0040, "Interdomain trust account"},
        //    {0x0080, "Workstation trust account"},
        //    {0x0100, "Server trust account"},
        //    {0x0200, "Password does not expire"},
        //    {0x0400, "Account auto locked" }
        //};

        public delegate void TextUpdate(string text);
        public event TextUpdate ErrorEvent;

        private List<int> _permutationMatrix = new List<int> { 0x8, 0x5, 0x4, 0x2, 0xb, 0x9, 0xd, 0x3, 0x0, 0x6, 0x1, 0xc, 0xe, 0xa, 0xf, 0x7 };
        private string _aqwerty = "!@#$%^&*()qwertyUIOPAzxcvbnmQQQQQQQQQQQQ)(*@&%\0";
        private string _anum = "0123456789012345678901234567890123456789\0";
        string _ntpassword = "NTPASSWORD\0";
        string _lmpassword = "LMPASSWORD\0";
        private static List<int> _oddParity = new List<int>{
1, 1, 2, 2, 4, 4, 7, 7, 8, 8, 11, 11, 13, 13, 14, 14,
16, 16, 19, 19, 21, 21, 22, 22, 25, 25, 26, 26, 28, 28, 31, 31,
32, 32, 35, 35, 37, 37, 38, 38, 41, 41, 42, 42, 44, 44, 47, 47,
49, 49, 50, 50, 52, 52, 55, 55, 56, 56, 59, 59, 61, 61, 62, 62,
64, 64, 67, 67, 69, 69, 70, 70, 73, 73, 74, 74, 76, 76, 79, 79,
81, 81, 82, 82, 84, 84, 87, 87, 88, 88, 91, 91, 93, 93, 94, 94,
97, 97, 98, 98,100,100,103,103,104,104,107,107,109,109,110,110,
112,112,115,115,117,117,118,118,121,121,122,122,124,124,127,127,
128,128,131,131,133,133,134,134,137,137,138,138,140,140,143,143,
145,145,146,146,148,148,151,151,152,152,155,155,157,157,158,158,
161,161,162,162,164,164,167,167,168,168,171,171,173,173,174,174,
176,176,179,179,181,181,182,182,185,185,186,186,188,188,191,191,
193,193,194,194,196,196,199,199,200,200,203,203,205,205,206,206,
208,208,211,211,213,213,214,214,217,217,218,218,220,220,223,223,
224,224,227,227,229,229,230,230,233,233,234,234,236,236,239,239,
241,241,242,242,244,244,247,247,248,248,251,251,253,253,254,254
                                                  };

        public string EmptyLm = "AAD3B435B51404EEAAD3B435B51404EE";
        public string EmptyNt = "31D6CFE0D16AE931B73C59D7E0C089C0";

        private string _samFile = string.Empty;
        private string _softwareFile = string.Empty;
        private string _systemFile = string.Empty;
        private List<UserAccount> _userAccounts = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="samFile"></param>
        /// <param name="softwareFile"></param>
        /// <param name="systemFile"></param>
        public SamParser(string samFile, 
                         string softwareFile, 
                         string systemFile)
        {
            _samFile = samFile;
            _softwareFile = softwareFile;
            _systemFile = systemFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserAccount> Parse()
        {
            _userAccounts = new List<UserAccount>();

            List<byte> bootKey = this.ExtractBootKey();
            if (bootKey == null)
            {
                return _userAccounts;
            }

            byte[] hashedBootKey = this.GenerateHashedBootKey(bootKey);
            if (hashedBootKey == null)
            {
                return _userAccounts;
            }

            if (this.EnumerateUsers(hashedBootKey) == false)
            {
                return _userAccounts;
            }

            List<UserGroup> userGroups = this.ExtractGroups();
            List<ProfilePath> profilePaths = this.ExtractProfilePaths();

            foreach (UserAccount userAccount in _userAccounts)
            {
                foreach (UserGroup userGroup in userGroups)
                {
                    string[] parts = userGroup.Rid.Split('-');
                    if (parts.Length > 0)
                    {
                        if (userAccount.Rid.ToString() == parts[parts.Length - 1])
                        {
                            userAccount.Groups += userGroup.Group + ",";
                        }
                    }
                }

                if (userAccount.Groups.Length > 0)
                {
                    if (userAccount.Groups.Substring(userAccount.Groups.Length - 1, 1) == ",")
                    {
                        userAccount.Groups = userAccount.Groups.Substring(0, userAccount.Groups.Length - 1);
                    }
                }
            }

            foreach (UserAccount userAccount in _userAccounts)
            {
                foreach (ProfilePath profilePath in profilePaths)
                {
                    string[] parts = profilePath.Rid.Split('-');
                    if (parts.Length > 0)
                    {
                        if (userAccount.Rid.ToString() == parts[parts.Length - 1])
                        {
                            userAccount.ProfilePath = profilePath.Path;
                        }
                    }
                }
            }

            return _userAccounts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<byte> ExtractBootKey()
        {
            try
            {
                RegParser regParser = new RegParser(_systemFile);

                RegKey rootKey = regParser.RootKey;

                StringBuilder output = new StringBuilder();

                RegKey regKey = rootKey.Key(@"Select");

                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: Select");
                    return null;
                }

                List<byte> bootKeyTemp = new List<byte>();

                RegValue regValueCcs = regKey.Value("Current");
                if (regValueCcs == null)
                {
                    this.OnError("Unable to locate the following registry key: Current");
                    return null;
                }

                regKey = rootKey.Key(@"ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\JD");
                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\JD");
                    return null;
                }

                string temp = regKey.ClassName;
                for (int i = 0; i < temp.Length / 2; i++)
                { 
                    bootKeyTemp.Add(Convert.ToByte(temp.Substring(i * 2, 2), 16));
                }

                regKey = rootKey.Key(@"ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\Skew1");
                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\Skew1");
                    return null;
                }

                temp = regKey.ClassName;
                for (int i = 0; i < temp.Length / 2; i++)
                {
                    bootKeyTemp.Add(Convert.ToByte(temp.Substring(i * 2, 2), 16));
                }

                regKey = rootKey.Key(@"ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\GBG");
                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\GBG");
                    return null;
                }

                temp = regKey.ClassName;
                for (int i = 0; i < temp.Length / 2; i++)
                {
                    bootKeyTemp.Add(Convert.ToByte(temp.Substring(i * 2, 2), 16));
                }

                regKey = rootKey.Key(@"ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\Data");
                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: ControlSet00" + regValueCcs.Data + "\\Control\\LSA\\Data");
                    return null;
                }

                temp = regKey.ClassName;
                for (int i = 0; i < temp.Length / 2; i++)
                {
                    bootKeyTemp.Add(Convert.ToByte(temp.Substring(i * 2, 2), 16));
                }

                List<byte> bootKey = new List<byte>();
                for (int index = 0; index < bootKeyTemp.Count; index++)
                {
                    bootKey.Add(bootKeyTemp[_permutationMatrix[index]]);
                }

                //this.PrintHex("Bootkey", bootKey.ToArray());

                return bootKey;
            }
            catch (Exception ex)
            {
                this.OnError("An error occured whilst extracting the boot key");
                Misc.WriteToEventLog(Application.ProductName, ex.Message, EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bootKey"></param>
        /// <returns></returns>
        private byte[] GenerateHashedBootKey(List<byte> bootKey)
        {
            try
            {
                RegParser regParser = new RegParser(_samFile);

                RegKey rootKey = regParser.RootKey;

                RegKey regKey = rootKey.Key(@"SAM\Domains\Account");

                if (regKey == null)
                {
                    this.OnError("Unable to locate the following registry key: SAM\\SAM\\Domains\\Account");
                    return null;
                }

                RegValue regValue = regKey.Value("F");
                if (regValue == null)
                {
                    this.OnError("Unable to locate the following registry key: SAM\\SAM\\Domains\\Account\\F");
                    return null;
                }

                byte[] hashedBootKey = new byte[16];

                Buffer.BlockCopy((byte[])regValue.Data, 112, hashedBootKey, 0, 16);

                //this.PrintHex("Hashed bootkey", hashedBootKey.ToArray());

                List<byte> data = new List<byte>();
                data.AddRange(hashedBootKey.ToArray());
                data.AddRange(Encoding.ASCII.GetBytes(_aqwerty));
                data.AddRange(bootKey.ToArray());
                data.AddRange(Encoding.ASCII.GetBytes(_anum));
                byte[] md5 = MD5.Create().ComputeHash(data.ToArray());

                byte[] encData = new byte[32];
                byte[] encOutput = new byte[32];

                Buffer.BlockCopy((byte[])regValue.Data, 128, encData, 0, 32);

                RC4Engine rc4Engine = new RC4Engine();
                rc4Engine.Init(true, new KeyParameter(md5));
                rc4Engine.ProcessBytes(encData, 0, 32, encOutput, 0);

                return encOutput;
            }
            catch (Exception ex)
            {
                this.OnError("An error occured whilst generating the hashed boot key");
                Misc.WriteToEventLog(Application.ProductName, ex.Message, EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedBootKey"></param>
        /// <returns></returns>
        private bool EnumerateUsers(byte[] hashedBootKey)
        {
            try
            {
                RegParser regParser = new RegParser(_samFile);

                RegKey rootKey = regParser.RootKey;

                RegKey regkey = rootKey.Key("SAM\\Domains\\Account\\Users");

                if (regkey == null)
                {
                    this.OnError("Unable to locate the following registry key: SAM\\Domains\\Account\\Users");
                    return false;
                }

                List<RegKey> regKeys = regkey.SubKeys;
                for (int indexKeys = 0; indexKeys < regKeys.Count; indexKeys++)
                {
                    RegValue regValue = regKeys[indexKeys].Value("V");

                    if (regValue == null)
                    {
                        continue;
                    }

                    UserAccount userAccount = new UserAccount();

                    string hexRid = Helper.RemoveHivePrefix(regKeys[indexKeys].Name).Replace("SAM\\Domains\\Account\\Users\\", string.Empty);
                    hexRid = Helper.RemoveHivePrefix(hexRid).Replace("SAM\\", string.Empty);
                    hexRid = Helper.RemoveHivePrefix(hexRid).Replace("ROOT\\", string.Empty);

                    byte[] data = (byte[])regValue.Data;

                    int offset = BitConverter.ToInt32((byte[])data, 12);
                    int length = BitConverter.ToInt32((byte[])data, 16);

                    offset += 204;

                    byte[] extract = new byte[length];
                    Buffer.BlockCopy(data, offset, extract, 0, length);

                    userAccount.LoginName = Helper.ReplaceNulls(Text.ConvertUnicodeToAscii(Text.ByteArray2String(extract, false, false)));

                    offset = BitConverter.ToInt32((byte[])data, 24);
                    length = BitConverter.ToInt32((byte[])data, 28);

                    offset += 204;

                    extract = new byte[length];
                    Buffer.BlockCopy(data, offset, extract, 0, length);

                    // Name
                    userAccount.Name = Helper.ReplaceNulls(Text.ConvertUnicodeToAscii(Text.ByteArray2String(extract, false, false)));

                    offset = BitConverter.ToInt32((byte[])data, 36);
                    length = BitConverter.ToInt32((byte[])data, 40);

                    offset += 204;

                    extract = new byte[length];
                    Buffer.BlockCopy(data, offset, extract, 0, length);

                    // Description
                    if (length > 0)
                    {
                        userAccount.Description = Helper.ReplaceNulls(Text.ConvertUnicodeToAscii(Text.ByteArray2String(extract, false, false)));
                    }

                    offset = BitConverter.ToInt32((byte[])data, 48);
                    length = BitConverter.ToInt32((byte[])data, 52);

                    offset += 204;

                    extract = new byte[length];
                    Buffer.BlockCopy(data, offset, extract, 0, length);

                    // User Comment
                    if (length > 0)
                    {
                        userAccount.UserComment = Helper.ReplaceNulls(Text.ConvertUnicodeToAscii(Text.ByteArray2String(extract, false, false)));
                    }

                    // LM Hash
                    offset = BitConverter.ToInt32((byte[])data, 156);
                    length = BitConverter.ToInt32((byte[])data, 160);

                    offset += 204;

                    userAccount.EncLmHash = new byte[length];

                    Buffer.BlockCopy(data, offset, userAccount.EncLmHash, 0, length);

                    // NT Hash
                    offset = BitConverter.ToInt32((byte[])data, 168);
                    length = BitConverter.ToInt32((byte[])data, 172);

                    offset += 204;

                    userAccount.EncNtHash = new byte[length];
                    Buffer.BlockCopy(data, offset, userAccount.EncNtHash, 0, length);

                    if (userAccount.EncLmHash.Length == 20)
                    {
                        byte[] tempLmHash = new byte[16];
                        Buffer.BlockCopy(userAccount.EncLmHash, 4, tempLmHash, 0, 16);
                        userAccount.EncLmHash = tempLmHash;
                        
                        this.DecryptHash(hexRid, hashedBootKey, _lmpassword, true, userAccount);
                    }
                    else
                    {
                        userAccount.Rid = Int32.Parse(hexRid, System.Globalization.NumberStyles.HexNumber);
                        Debug.WriteLine("**LMLEN**: " + userAccount.EncLmHash.Length);
                    }

                    if (userAccount.EncNtHash.Length == 20)
                    {
                        byte[] tempNtHash = new byte[16];
                        Buffer.BlockCopy(userAccount.EncNtHash, 4, tempNtHash, 0, 16);
                        userAccount.EncNtHash = tempNtHash;

                        this.DecryptHash(hexRid, hashedBootKey, _ntpassword, false, userAccount);
                    }
                    else
                    {
                        userAccount.Rid = Int32.Parse(hexRid, System.Globalization.NumberStyles.HexNumber);
                        Debug.WriteLine("**NTLEN**: " + userAccount.EncNtHash.Length);
                    }

                    // F
                    regValue = regKeys[indexKeys].Value("F");

                    data = (byte[])regValue.Data;

                    extract = new byte[8];
                    Buffer.BlockCopy(data, 8, extract, 0, 8);

                    long i = BitConverter.ToInt64((byte[])extract, 0);

                    DateTime dateTime = DateTime.FromFileTimeUtc(i);

                    userAccount.LastLoginDate = dateTime.ToShortTimeString() + " " + dateTime.ToShortDateString();

                    extract = new byte[8];
                    Buffer.BlockCopy(data, 24, extract, 0, 8);

                    i = BitConverter.ToInt64((byte[])extract, 0);

                    dateTime = DateTime.FromFileTimeUtc(i);

                    userAccount.PasswordResetDate = dateTime.ToShortTimeString() + " " + dateTime.ToShortDateString();

                    extract = new byte[8];
                    Buffer.BlockCopy(data, 32, extract, 0, 8);

                    i = BitConverter.ToInt64((byte[])extract, 0);

                    if (i == 0 | i == 9223372036854775807)
                    {
                        userAccount.AccountExpiryDate = string.Empty;
                    }
                    else
                    {
                        dateTime = DateTime.FromFileTimeUtc(i);

                        userAccount.AccountExpiryDate = dateTime.ToShortTimeString() + " " + dateTime.ToShortDateString();
                    }

                    extract = new byte[8];
                    Buffer.BlockCopy(data, 40, extract, 0, 8);

                    i = BitConverter.ToInt64((byte[])extract, 0);

                    dateTime = DateTime.FromFileTimeUtc(i);

                    userAccount.LoginFailedDate = dateTime.ToShortTimeString() + " " + dateTime.ToShortDateString();

                    //extract = new byte[4];
                    //Buffer.BlockCopy(data, 48, extract, 0, 4);

                    //i = BitConverter.ToInt32((byte[])extract, 0);

                    //output.AppendLine("RID: " + i);

                    extract = new byte[2];
                    Buffer.BlockCopy(data, 56, extract, 0, 2);

                    i = BitConverter.ToInt16((byte[])extract, 0);
                    if ((i & (int)0x0001) == 0x0001)
                    {
                        userAccount.IsDisabled = true;
                    }

                    //foreach (DictionaryEntry de in _acbs)
                    //{
                    //    if ((i & (int)de.Key) == (int)de.Key)
                    //    {
                    //        output.AppendLine(de.Value.ToString());
                    //    }
                    //}

                    extract = new byte[2];
                    Buffer.BlockCopy(data, 64, extract, 0, 2);

                    i = BitConverter.ToInt16((byte[])extract, 0);

                    userAccount.FailedLogins = i;

                    extract = new byte[2];
                    Buffer.BlockCopy(data, 66, extract, 0, 2);

                    i = BitConverter.ToInt16((byte[])extract, 0);

                    userAccount.LoginCount = i;

                    _userAccounts.Add(userAccount);
                }

                return true;
            }
            catch (Exception ex)
            {
                this.OnError("An error occured whilst enumerating users");
                Misc.WriteToEventLog(Application.ProductName, "An error occured whilst enumerating users: " + ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexRid"></param>
        /// <param name="hashedBootKey"></param>
        /// <param name="lmntpassword"></param>
        /// <param name="isLmHash"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        private bool DecryptHash(string hexRid, 
                                 byte[] hashedBootKey, 
                                 string lmntpassword, 
                                 bool isLmHash, 
                                 UserAccount userAccount)
        {
            try
            {
                userAccount.Rid = Int32.Parse(hexRid, System.Globalization.NumberStyles.HexNumber);

                byte[] tempBootKey = new byte[16];
                Buffer.BlockCopy(hashedBootKey, 0, tempBootKey, 0, 16);

                List<byte> data = new List<byte>();
                data.AddRange(tempBootKey.ToArray());
                data.AddRange(MiscUtil.Conversion.EndianBitConverter.Little.GetBytes(Int32.Parse(hexRid, System.Globalization.NumberStyles.HexNumber)));
                data.AddRange(Encoding.ASCII.GetBytes(lmntpassword));
                byte[] md5 = MD5.Create().ComputeHash(data.ToArray());

                //PrintHex(md5);

                byte[] encOutput = new byte[16];

                RC4Engine rc4Engine = new RC4Engine();
                if (isLmHash == true)
                {
                    rc4Engine.Init(true, new KeyParameter(md5));
                    rc4Engine.ProcessBytes(userAccount.EncLmHash, 0, 16, encOutput, 0);
                }
                else
                {
                    rc4Engine.Init(true, new KeyParameter(md5));
                    rc4Engine.ProcessBytes(userAccount.EncNtHash, 0, 16, encOutput, 0);
                }

                //PrintHex(encOutput);

                byte[] hashBytes1 = new byte[8];
                byte[] hashBytes2 = new byte[8];
                Buffer.BlockCopy(encOutput, 0, hashBytes1, 0, 8);
                Buffer.BlockCopy(encOutput, 8, hashBytes2, 0, 8);

                List<int> key1Temp = new List<int>();
                List<int> key2Temp = new List<int>();

                this.SidToKey(hexRid, ref key1Temp, ref key2Temp);

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

                List<byte> key1 = new List<byte>();
                foreach (int val in key1Temp)
                {
                    key1.Add((byte)val);
                }

                List<byte> key2 = new List<byte>();
                foreach (int val in key2Temp)
                {
                    key2.Add((byte)val);
                }

                cryptoProvider.Padding = PaddingMode.None;
                cryptoProvider.Mode = CipherMode.ECB;
                ICryptoTransform transform = cryptoProvider.CreateDecryptor(key1.ToArray(), new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
                MemoryStream memoryStream = new MemoryStream(hashBytes1);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[hashBytes1.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                PrintHex("HASH:", plainTextBytes);

                if (isLmHash == true)
                {
                    userAccount.LmHash = Text.ConvertByteArrayToHexString(plainTextBytes);
                }
                else
                {
                    userAccount.NtHash = Text.ConvertByteArrayToHexString(plainTextBytes);
                }
                
                //PrintHex(plainTextBytes);

                cryptoProvider.Padding = PaddingMode.None;
                cryptoProvider.Mode = CipherMode.ECB;
                transform = cryptoProvider.CreateDecryptor(key2.ToArray(), new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
                memoryStream = new MemoryStream(hashBytes2);
                cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
                plainTextBytes = new byte[hashBytes2.Length];
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                if (isLmHash == true)
                {
                    userAccount.LmHash += Text.ConvertByteArrayToHexString(plainTextBytes);
                }
                else
                {
                    userAccount.NtHash += Text.ConvertByteArrayToHexString(plainTextBytes);
                }

                //PrintHex(plainTextBytes);

                return true;
            }
            catch (Exception ex)
            {
                this.OnError("An error occured whilst decrypting the user hash");
                Misc.WriteToEventLog(Application.ProductName, "An error occured whilst decrypting the user hash: " + ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexRid"></param>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        private void SidToKey(string hexRid, 
                              ref List<int> key1, 
                              ref List<int> key2)
        {
            int rid = Int32.Parse(hexRid, System.Globalization.NumberStyles.HexNumber);

            List<byte> temp1 = new List<byte>();

            byte temp = (byte)(rid & 0xFF);
            temp1.Add(temp);

            temp = (byte)(((rid >> 8) & 0xFF));
            temp1.Add(temp);

            temp = (byte)(((rid >> 16) & 0xFF));
            temp1.Add(temp);

            temp = (byte)(((rid >> 24) & 0xFF));
            temp1.Add(temp);

            temp1.Add(temp1[0]);
            temp1.Add(temp1[1]);
            temp1.Add(temp1[2]);

            //PrintHex(temp1.ToArray());

            List<byte> temp2 = new List<byte>();
            temp2.Add(temp1[3]);
            temp2.Add(temp1[0]);
            temp2.Add(temp1[1]);
            temp2.Add(temp1[2]);

            temp2.Add(temp2[0]);
            temp2.Add(temp2[1]);
            temp2.Add(temp2[2]);

            //PrintHex(temp2.ToArray());

            key1 = StrToKey(temp1);
            key2 = StrToKey(temp2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<int> StrToKey(List<byte> data)
        {
            List<int> temp = new List<int>();

            temp.Add(data[0] >> 1);
            temp.Add(((data[0] & 0x01) << 6) | (data[1] >> 2));
            temp.Add(((data[1] & 0x03) << 5) | (data[2] >> 3));
            temp.Add(((data[2] & 0x07) << 4) | (data[3] >> 4));
            temp.Add(((data[3] & 0x0f) << 3) | (data[4] >> 5));
            temp.Add(((data[4] & 0x1f) << 2) | (data[5] >> 6));
            temp.Add(((data[5] & 0x3f) << 1) | (data[6] >> 7));
            temp.Add(data[6] & 0x7f);

            for (int index = 0; index < 8; index++)
            {
                temp[index] = (temp[index] << 1);
                temp[index] = _oddParity[temp[index]];
            }

            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ProfilePath> ExtractProfilePaths()
        {
            RegParser regParser = new RegParser(_softwareFile);

            RegKey rootKey = regParser.RootKey;

            RegKey regKey = rootKey.Key("Microsoft\\Windows NT\\CurrentVersion\\ProfileList");

            if (regKey == null)
            {
                Console.WriteLine("Unable to locate the following registry key: Microsoft\\Windows NT\\CurrentVersion\\ProfileList");
                return null;
            }

            List<ProfilePath> profilePaths = new List<ProfilePath>();
            List<RegKey> subKeys = regKey.SubKeys;
            for (int index = 0; index < subKeys.Count; index++)
            {
                RegValue regValue = subKeys[index].Value("ProfileImagePath");
                if (regValue == null)
                {
                    continue;
                }

                ProfilePath profilePath = new ProfilePath();
                string temp = regValue.Data.ToString().Replace("\0", string.Empty);
                profilePath.Path = temp;
                profilePath.Rid = woanware.Helper.RemoveHivePrefix(subKeys[index].Name).Replace("Microsoft\\Windows NT\\CurrentVersion\\ProfileList\\", string.Empty);

                profilePaths.Add(profilePath);
            }

            return profilePaths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<UserGroup> ExtractGroups()
        {
            RegParser regParser = new RegParser(_samFile);

            RegKey rootKey = regParser.RootKey;

            RegKey regKey = rootKey.Key("SAM\\Domains\\Builtin\\Aliases");
            if (regKey == null)
            {
                Console.WriteLine("Unable to locate the following registry key: SAM\\Domains\\Builtin\\Aliases");
                return null;
            }

            List<UserGroup> userGroups = new List<UserGroup>();
            List<RegKey> regKeys = regKey.SubKeys;
            for (int indexKeys = 0; indexKeys < regKeys.Count; indexKeys++)
            {
                //Console.WriteLine(regKeys[indexKeys].Name);

                RegValue regValue = regKeys[indexKeys].Value("C");

                if (regValue == null)
                {
                    continue;
                }

                byte[] data = (byte[])regValue.Data;

                int offset = BitConverter.ToInt32((byte[])data, 16);
                int length = BitConverter.ToInt32((byte[])data, 20);

                offset += 52;

                byte[] extract = new byte[length];
                Buffer.BlockCopy(data, offset, extract, 0, length);

                // Group Name
                //Console.WriteLine("Group Name: " + Functions.ConvertUnicodeToAscii(Functions.ByteArray2String(extract, false, false)));
                string groupName = Text.ConvertUnicodeToAscii(Text.ByteArray2String(extract, false, false));

                offset = BitConverter.ToInt32((byte[])data, 28);
                length = BitConverter.ToInt32((byte[])data, 32);

                offset += 52;

                extract = new byte[length];
                Buffer.BlockCopy(data, offset, extract, 0, length);

                // Comment
                //Console.WriteLine("Comment: " + Functions.ConvertUnicodeToAscii(Functions.ByteArray2String(extract, false, false)));

                int users = BitConverter.ToInt32((byte[])data, 48);

                // No. Users
                //Console.WriteLine("No. Users: " + users);

                offset = BitConverter.ToInt32((byte[])data, 40);
                length = BitConverter.ToInt32((byte[])data, 44);

                if (users > 0)
                {
                    int count = 0;
                    for (int indexUsers = 1; indexUsers <= users; indexUsers++)
                    {
                        int dataOffset = offset + 52 + count;
                        int sidType = BitConverter.ToInt32((byte[])data, dataOffset);

                        UserGroup userGroup = new UserGroup();

                        if (sidType == 257)
                        {
                            extract = new byte[12];

                            Buffer.BlockCopy(data, dataOffset, extract, 0, 12);

                            int rev = extract[0];
                            int dashes = BitConverter.ToInt32((byte[])extract, 1);

                            byte[] ntauthData = new byte[8];
                            Buffer.BlockCopy(extract, 2, ntauthData, 2, 6);

                            string ntauth = Text.ConvertByteArrayToHexString(ntauthData);

                            ntauth = Regex.Replace(ntauth, "^0+", string.Empty);

                            byte[] ntnonunique = new byte[4];
                            Buffer.BlockCopy(extract, 8, ntnonunique, 0, 4);
                            uint intntnonunique = BitConverter.ToUInt32((byte[])ntnonunique, 0);

                            //Console.WriteLine("S-" + rev + "-" + ntauth + "-" + intntnonunique);

                            userGroup.Group = groupName;
                            userGroup.Rid = "S-" + rev + "-" + ntauth + "-" + intntnonunique;

                            userGroups.Add(userGroup);

                            count += 12;
                        }
                        else if (sidType == 1281)
                        {
                            extract = new byte[26];

                            Buffer.BlockCopy(data, dataOffset, extract, 0, 26);

                            int rev = extract[0];
                            int dashes = BitConverter.ToInt32((byte[])extract, 1);

                            byte[] ntauthData = new byte[8];
                            Buffer.BlockCopy(extract, 2, ntauthData, 2, 6);

                            string ntauth = Text.ConvertByteArrayToHexString(ntauthData);

                            ntauth = Regex.Replace(ntauth, "^0+", string.Empty);

                            byte[] ntnonunique = new byte[4];
                            Buffer.BlockCopy(extract, 8, ntnonunique, 0, 4);
                            uint intntnonunique = BitConverter.ToUInt32((byte[])ntnonunique, 0);

                            byte[] part1 = new byte[4];
                            Buffer.BlockCopy(extract, 12, part1, 0, 4);
                            uint intpart1 = BitConverter.ToUInt32((byte[])part1, 0);

                            byte[] part2 = new byte[4];
                            Buffer.BlockCopy(extract, 16, part2, 0, 4);
                            uint intpart2 = BitConverter.ToUInt32((byte[])part2, 0);

                            byte[] part3 = new byte[4];
                            Buffer.BlockCopy(extract, 20, part3, 0, 4);
                            uint intpart3 = BitConverter.ToUInt32((byte[])part3, 0);

                            byte[] rid = new byte[4];
                            Buffer.BlockCopy(extract, 24, rid, 0, 2);
                            int intrid = BitConverter.ToInt32((byte[])rid, 0);

                            //Console.WriteLine("S-" + rev + "-" + ntauth + "-" + intntnonunique + "-" + intpart1 + "-" + intpart2 + "-" + intpart3 + "-" + intrid);

                            userGroup.Group = groupName;
                            userGroup.Rid = "S-" + rev + "-" + ntauth + "-" + intntnonunique + "-" + intpart1 + "-" + intpart2 + "-" + intpart3 + "-" + intrid;

                            userGroups.Add(userGroup);

                            count += 28;
                        }
                    }
                }
            }

            return userGroups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        private void PrintHex(string name, byte[] data)
        {
            StringBuilder hex = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                hex.Append(data[i].ToString("x2"));
            }

            Debug.WriteLine(name + ": " + hex.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void OnError(string text)
        {
            if (ErrorEvent != null)
            {
                ErrorEvent(text);
            }
        }
    }
}
