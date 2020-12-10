﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Partials
{
    public abstract class Baseclass : IDataErrorInfo, INotifyPropertyChanged
    {
        /*[NotMapped]
        public virtual string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                var property = GetType().GetProperty(columnName);
                if (property != null)

                {
                    var validationContext = new ValidationContext(this)
                    {
                        MemberName = columnName
                    };

                    var isVal = Validator.TryValidateProperty(property.GetValue(this), validationContext, validationResults);
                    if (isVal)
                    {
                        return null;
                    }

                    return validationResults.First().ErrorMessage;
                }
                return "";
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        /*[NotMapped]
        public string Error
        {
            get
            {
                string foutmeldingen = "";

                foreach (var item in this.GetType().GetProperties(BindingFlags.DeclaredOnly)) //reflection 
                {

                    string fout = this[item.Name];
                    if (!string.IsNullOrWhiteSpace(fout))
                    {
                        foutmeldingen += fout + Environment.NewLine;
                    }

                }
                return foutmeldingen;
            }
        }*/
        [NotMapped]
        public virtual string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                var property = GetType().GetProperty(columnName);
                Contract.Assert(null != property);

                var validationContext = new ValidationContext(this)
                {
                    MemberName = columnName
                };

                var isValid = Validator.TryValidateProperty(property.GetValue(this), validationContext, validationResults);
                if (isValid)
                {
                    return null;
                }

                return validationResults.First().ErrorMessage;
            }
        }
        [NotMapped]
        public virtual string Error
        {
            get
            {
                var propertyInfos = GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);//.Where(x => x.CanWrite == true);

                foreach (var propertyInfo in propertyInfos)
                {
                    var errorMsg = this[propertyInfo.Name];
                    if (null != errorMsg)
                    {
                        return errorMsg;
                    }
                }

                return null;
            }
        }
    }
}
