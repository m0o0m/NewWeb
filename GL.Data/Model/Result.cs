using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GL.Data.Model
{
    public class Result
    {
        private string _msg = "";
        private int _err = 0;
        private string _value = "";


        public string msg
        {
            get { return _msg; }
            set { value = _msg; }
        }

        public int err
        {
            get { return _err; }
            set { value = _err; }
        }

        public string value
        {
            get { return _value; }
            set { value = _value; }
        }


        /// <summary>
        /// 未知错误
        /// </summary>
        public static int UnknownError = -1;
        /// <summary>
        /// 通用值 成功
        /// </summary>
        public static int Normal = 0;
        /// <summary>
        /// 通用值 失败
        /// </summary>
        public static int Lost = 1;
        /// <summary>
        /// 操作失败，影响行数为0
        /// </summary>
        public static int ResultError = 2;
        /// <summary>
        /// 操作失败，影响行数为预期之外的值
        /// </summary>
        public static int ResultExcept = 3;
        /// <summary>
        /// 值不能为空
        /// </summary>
        public static int ValueCanNotBeNull = 200;
        /// <summary>
        /// 用户不存在
        /// </summary>
        public static int UserDoesNotExist = 201;
        /// <summary>
        /// 密码错误
        /// </summary>
        public static int PasswordIsIncorrect = 202;
        /// <summary>
        /// 值的长度不够
        /// </summary>
        public static int ValueIsTooLong = 203;

        /// <summary>
        /// 账号已存在
        /// </summary>
        public static int AccountHasBeenRegistered = 204;
        /// <summary>
        /// 账号只能由字母和数字组成
        /// </summary>
        public static int AccountOnlyConsistOfLettersAndNumbers = 205;
        /// <summary>
        /// 值的长度不相符
        /// </summary>
        public static int TheLengthOfTheValueIsNotConsistent = 206;
        /// <summary>
        /// 联系电话有误
        /// </summary>
        public static int PhoneIsWrong = 207;
        /// <summary>
        /// 邮箱地址有误
        /// </summary>
        public static int EmailIsWrong = 208;
        /// <summary>
        /// 值的长度不够
        /// </summary>
        public static int ValueIsTooBiger = 209;

        /// <summary>
        /// 重定向
        /// </summary>
        public static int Redirect = 210;

        /// <summary>
        /// 值的不是数字
        /// </summary>
        public static int ValueIsNotNumber = 211;


        /// <summary>
        /// 参数格式错误
        /// </summary>
        public static int ParaFormError = 212;

        /// <summary>
        /// 参数格式错误
        /// </summary>
        public static int ParaYZMError = 213;

        /// <summary>
        /// 参数格式错误
        /// </summary>
        public static int ParaErrorCount = 214;

        /// <summary>
        /// 找不到页面
        /// </summary>
        public static int Err404 = 404;
        /// <summary>
        /// 找不到页面
        /// </summary>
        public static int Err403 = 403;
        /// <summary>
        /// 找不到页面
        /// </summary>
        public static int Err405 = 405;
        /// <summary>
        /// 找不到页面
        /// </summary>
        public static int Err500 = 500;

        /// <summary>
        /// 账号的下级代理必须为空
        /// </summary>
        public static int AccountOfTheLowerAgentMustBeEmpty = 10200;
        /// <summary>
        /// 账号的下级会员必须为空
        /// </summary>
        public static int AccountOfTheLowerMemberMustBeEmpty = 10201;
        /// <summary>
        /// 超出数值范围
        /// </summary>
        public static int BeyondTheScopeOfNumerical = 10202;


    }
}
