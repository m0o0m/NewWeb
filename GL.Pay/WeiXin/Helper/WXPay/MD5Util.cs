namespace GL.Pay.WxPay.Helper.WXPay
{
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Security.Cryptography;
	
	public class MD5Util {
		public static String MD5(String s) {
			char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
					'A', 'B', 'C', 'D', 'E', 'F' };
			try {

                byte[] btInput = System.Text.Encoding.UTF8.GetBytes(s);
				// ���MD5ժҪ�㷨�� MessageDigest ����
				MD5 mdInst = System.Security.Cryptography.MD5.Create();
				// ʹ��ָ�����ֽڸ���ժҪ
				mdInst.ComputeHash(btInput);
				// �������
				byte[] md = mdInst.Hash;
				// ������ת����ʮ�����Ƶ��ַ�����ʽ
				int j = md.Length;
				char[] str = new char[j * 2];
				int k = 0;
				for (int i = 0; i < j; i++) {
					byte byte0 = md[i];
					str[k++] = hexDigits[(int) (((byte) byte0) >> 4) & 0xf];
					str[k++] = hexDigits[byte0 & 0xf];
				}
                return new string(str); 
			} catch (Exception e) {
				Console.Error.WriteLine(e.StackTrace);
				return null;
			}
		}
	}
}
