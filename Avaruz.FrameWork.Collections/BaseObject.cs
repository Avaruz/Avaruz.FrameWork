using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Avaruz.FrameWork.Collections
{
    /// <summary>
    /// Summary description for BaseObject.
    /// </summary>
    /// 
    [Serializable()]
    public abstract class BaseObject : ICloneable, IEditableObject, INotifyPropertyChanged, IDataErrorInfo
    {

        private List<Rule> _rules;
        public enum ObjectStateType { Unchanged = 0, Added = 1, Changed = 2, Deleted = 3 };
        public BaseObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Type TypeOfObject
        {
            get { return this.GetType(); }
        }
        public void MarkForDeletion()
        {
            this.ObjectState = ObjectStateType.Deleted;
        }
        public CustomCollection Parent = null;
        private bool _ispersistent = false;
        public bool IsPersistent
        {
            get { return _ispersistent; }
            set { _ispersistent = value; }
        }
        private ObjectStateType _ObjectState = ObjectStateType.Unchanged;
        public ObjectStateType ObjectState
        {
            get { return _ObjectState; }
            set { _ObjectState = value; }
        }
        private bool _IsDirty;
        public bool IsDirty
        {
            get
            { return _IsDirty; }
            set { _IsDirty = value; }
        }
        public int Compare(object obj1, object obj2)
        {
            try
            {
                int Rt = 0;
                if (obj1 == null && obj2 == null)
                {
                    Rt = 0;
                }
                else if (obj1 == null && obj2 != null)
                {
                    Rt = -1;
                }
                else if (obj1 != null & obj2 == null)
                {
                    Rt = 1;
                }
                else if (obj1 == obj2)
                {
                    Rt = 0;
                }
                else
                {
                    if (obj1.Equals(obj2))
                    {
                        Rt = 0;
                    }
                    else
                    {
                        Rt = 1;
                    }
                }
                return Rt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #region IEditableObject
        // Implements IEditableObject
        private bool inTxn = false;
        private bool IsNew = true;
        public void BeginEdit()
        {
            if (!inTxn)
            {
                inTxn = true;

            }
        }
        public void EndEdit()
        {
            inTxn = false;
            IsNew = false;
        }
        public void CancelEdit()
        {
            inTxn = false;
            if (IsNew)
            {
                IsNew = false;
                if (Parent != null)
                {
                    this.Parent.Remove(this);
                }
            }
        }
        #endregion
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void SetDirtyFlag()
        {

            this.IsDirty = true;
            if (this.Parent != null)
            {
                this.Parent.CollectionChanged(this);
            }
            if (this.IsPersistent)//l'object existe dans la base
            {
                this.ObjectState = ObjectStateType.Changed;//objet changé
            }
            else
            {
                this.ObjectState = ObjectStateType.Added;//objet ajouté
            }

        }

        /// <summary>
        /// Gets a value indicating whether or not this domain object is valid. 
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return this.Error == null;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this domain object. The default is an empty string ("").
        /// </summary>
        public virtual string Error
        {
            get
            {
                string result = this[string.Empty];
                if (result != null && result.Trim().Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public virtual string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName = CleanString(propertyName);

                foreach (Rule r in GetBrokenRules(propertyName))
                {
                    if (propertyName == string.Empty || r.PropertyName == propertyName)
                    {
                        result += r.Description;
                        result += Environment.NewLine;
                    }
                }
                result = result.Trim();
                if (result.Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }

        /// <summary>
        /// Validates all rules on this domain object, returning a list of the broken rules.
        /// </summary>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<Rule> GetBrokenRules()
        {
            return GetBrokenRules(string.Empty);
        }

        /// <summary>
        /// Validates all rules on this domain object for a given property, returning a list of the broken rules.
        /// </summary>
        /// <param name="property">The name of the property to check for. If null or empty, all rules will be checked.</param>
        /// <returns>A read-only collection of rules that have been broken.</returns>
        public virtual ReadOnlyCollection<Rule> GetBrokenRules(string property)
        {
            property = CleanString(property);

            // If we haven't yet created the rules, create them now.
            if (_rules == null)
            {
                _rules = new List<Rule>();
                _rules.AddRange(this.CreateRules());
            }
            List<Rule> broken = new List<Rule>();


            foreach (Rule r in this._rules)
            {
                // Ensure we only validate a rule 
                if (r.PropertyName == property || property == string.Empty)
                {
                    bool isRuleBroken = !r.ValidateRule(this);
                    if (isRuleBroken)
                    {
                        broken.Add(r);
                    }
                }
            }

            return broken.AsReadOnly();
        }

        /// <summary>
        /// Occurs when any properties are changed on this object.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Override this method to create your own rules to validate this business object. These rules must all be met before 
        /// the business object is considered valid enough to save to the data store.
        /// </summary>
        /// <returns>A collection of rules to add for this business object.</returns>
        protected virtual List<Rule> CreateRules()
        {
            return new List<Rule>();
        }

        /// <summary>
        /// A helper method that raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name="propertyNames">The names of the properties that changed.</param>
        protected virtual void NotifyChanged(params string[] propertyNames)
        {
            foreach (string name in propertyNames)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(name));
            }
            OnPropertyChanged(new PropertyChangedEventArgs("IsValid"));
        }

        /// <summary>
        /// Cleans a string by ensuring it isn't null and trimming it.
        /// </summary>
        /// <param name="s">The string to clean.</param>
        protected string CleanString(string s)
        {
            return (s ?? string.Empty).Trim();
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }


    }
}
