using Koo.Utilities.Data;
using Koo.Utilities.Helpers;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YiSha.Model.Publishes
{
    public partial class PublishedVar:NotifyObject
    {
        public string HtmlId
        {
            get
            {
                return this.VarName.Replace(".", "_");
            }
        }
   
        private string _Name;
        /// <summary>
        /// 标题（可作为属性页面里面的属性）
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetProperty(ref _Name, value);
            }
        }
        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark
        {
            get
            {
                return _Description;
            }
            set
            {
                SetProperty(ref _Description, value);
            }
        }

        #region 变量

        /// <summary>
        /// 变量作用域
        /// </summary>
        public VariableScope Scope
        {
            get;set;
        }

        private string _VarName;
        /// <summary>
        /// 变量名称
        /// </summary>
        public string VarName
        {
            get
            {
                return _VarName;
            }
            set
            {
                SetProperty(ref _VarName, value);
            }
        }
        #endregion

        ///// <summary>
        ///// 这个里面显示的变量肯定都是显示声明的
        ///// </summary>
        //public VariableDeclareType DeclareType
        //{
        //    get;set;
        //}
        /// <summary>
        /// 是否内置的变量
        /// </summary>
        public bool IsBuildIn
        {
            get;set;
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get;set;
        }
        /// <summary>
        /// 变量值类型
        /// </summary>
        /// <summary>
        /// 变量值类型
        /// </summary>
        public VariableDataType DataType
        {
            get; set;
        }

        public InOutDirectionEnumType Direction
        {
            get; set;
        }

        private string _DefaultValue;
        /// <summary>
        /// 当前设置的值
        /// </summary>
        public string Value
        {
            get
            {
                return _DefaultValue;
            }
            set
            {
                SetProperty(ref _DefaultValue, value);
            }
        }
        //private string _rawValue;
        
        //public string DecryptedValue
        //{
        //    get
        //    {
        //        //if (this.DataType == VariableDataType.Password)
        //        //{
        //        //    this._rawValue = EncryptionService.Instance.DecryptVarPassword(Value);
        //        //}
        //        return _rawValue;
        //    }
        //    set
        //    {
        //        _rawValue = value;
        //        //if (this.DataType == VariableDataType.Password)
        //        //{
        //        //    this.Value = EncryptionService.Instance.EncryptVarPassword(value);
        //        //}
        //        this.OnPropertyChanged();
        //    }
        //}
        private VarValueSource _valueSource = VarValueSource.Local;
        public VarValueSource ValueSource
        {
            get
            {
                return this._valueSource;
            }
            set
            {
                this.SetProperty(ref _valueSource, value);
            }
        }
     
        private int _SortNum;
        public int SortNum
        {
            get
            {
                return _SortNum;
            }
            set
            {
                SetProperty(ref _SortNum, value);
            }
        }
        private bool _isSelected;
        /// <summary>
        /// 这个需要序列化，因为在序列化复制之后，需要知道是否有选中
        /// </summary>
        public  bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                this.SetProperty(ref _isSelected, value);
            }
        }

    }

    public partial class PublishedVar
    {
        /// <summary>
        /// 默认值，上级的值
        /// </summary>
    
        public string DefaultValue
        {
            get;set;
        }
        public bool IsDefaultValue
        {
            get
            {
                return this.ValueSource == VarValueSource.Inherit;
            }
        }
    }
    public class ReleasedVarCollection : List<PublishedVar>
    {
        public ReleasedVarCollection()
        {
        }
        public ReleasedVarCollection(List<PublishedVar> items) : base(items)
        {
        }

        public PublishedVar GetByVarName(string varName)
        {
            return this.FirstOrDefault(x => x.VarName == varName);
        }
    }
}
