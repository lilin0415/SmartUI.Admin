using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Publishes
{
    public class SavingVarItem
    {
        /// <summary>
        /// 当前变量所属的文档ID
        /// </summary>
        public string DocId
        {
            get; set;
        }
        /// <summary>
        /// 变量名称
        /// </summary>
        public string VarName
        {
            get; set;
        }
        /// <summary>
        /// 变量的值
        /// </summary>
        public string Value
        {
            get; set;
        }

        /// <summary>
        /// 变量的数据类型
        /// </summary>
        public VariableDataType DataType
        {
            get;set;
        }

        /// <summary>
        /// 是否使用默认值
        /// </summary>
        public string IsDefaultValue
        {
            get; set;
        }

        public VarValueSource ValueSource
        {
            get
            {
                return IsDefaultValue == "true" ? VarValueSource.Inherit : VarValueSource.Local;
            }
        }
    }
}
