using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Entity
{
    public class DbFieldAttribute:Attribute
    {
        public string FieldName
        {
            get; set;
        }

        public string DataType
        {
            get; set;
        }

        public string FieldLength
        {
            get; set;
        }

        public string IsNullable
        {
            get; set;
        }
        public string TableIdentity
        {
            get; set;
        }
        public string Key
        {
            get; set;
        }
        public string FieldDefault
        {
            get; set;
        }
        public string Remark
        {
            get; set;
        }
        public DbFieldAttribute(string fieldName, string dataType, string TableIdentity, string key, string isNullable, string FieldDefault)
        {
            FieldName = fieldName;
            DataType = dataType;
            this.TableIdentity = TableIdentity;
            this.Key = key;
            this.IsNullable = isNullable;
            this.FieldDefault = FieldDefault;
        }   
    }
}
