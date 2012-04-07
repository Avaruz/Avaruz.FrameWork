using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace Avaruz.FrameWork.Collections
{
    public abstract class CustomCollection : CollectionBase, ICloneable, IBindingList
    {
        public CustomCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region IClonable
        public object Clone()
        {
            Object Rt = Activator.CreateInstance(this.GetType());
            for (int i = 0; i < this.Count; i++)
            {
                ((CustomCollection)Rt).InnerList.Add(((BaseObject)this[i]).Clone());
            }
            //((CustomCollection )Rt).FiltredItems = (CustomCollection) this.FiltredItems.Clone();
            //deleteditem à ajouter
            return Rt;
        }
        #endregion
        #region My Methods
        public bool IsDirty
        {
            get
            {
                bool Rt = false;
                if (((CustomCollection)this.GetChanges()).Count != 0)
                { Rt = true; }
                return Rt;
            }
        }
        private Type _ItemType;
        public Type ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }
        public void Sort(string Expression)
        {
            try
            {
                string[] SplittedExpr = Expression.Split(',');
                string PropertyName;
                ListSortDirection Direction;
                for (int i = 0; i <= SplittedExpr.GetUpperBound(0); i++)
                {

                    if (-SplittedExpr[i].ToUpper().IndexOf(" ASC") < 0)
                    {
                        PropertyName = SplittedExpr[i].Replace(" ASC", "").Trim();
                        Direction = ListSortDirection.Ascending;
                    }
                    else if (-SplittedExpr[i].ToUpper().IndexOf(" DESC") < 0)
                    {
                        PropertyName = SplittedExpr[i].Replace(" DESC", "").Trim();
                        Direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        PropertyName = SplittedExpr[i].Trim();
                        Direction = ListSortDirection.Ascending;
                    }
                    base.InnerList.Sort(new ObjectPropertyComparer(PropertyName));
                    if (Direction == ListSortDirection.Descending) base.InnerList.Reverse();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private bool ischangescontainer = false;
        public bool IsChangesContainer
        {
            get { return ischangescontainer; }
            set { ischangescontainer = value; }
        }
        private bool _IsFiltering = false;
        public bool IsFiltering
        {
            get { return _IsFiltering; }
            set { _IsFiltering = value; }
        }

        public CustomCollection GetChanges()
        {
            try
            {
                object Rt = Activator.CreateInstance(this.GetType());

                //Création d'une collection complète
                object WholeCollection = Activator.CreateInstance(this.GetType());

                //clonage 
                WholeCollection = (CustomCollection)this.Clone();

                //Ajout des lments filtrs

                for (int i = 0; i < this.FiltredItems.Count; i++)
                {
                    ((CustomCollection)WholeCollection).Add(this.FiltredItems[i]);
                }


                //Eléments Ajoutés et modifiés
                for (int i = 0; i < ((CustomCollection)WholeCollection).Count; i++)
                {
                    if (((BaseObject)((CustomCollection)WholeCollection)[i]).ObjectState != BaseObject.ObjectStateType.Unchanged)
                    {
                        ((CustomCollection)Rt).Add(((CustomCollection)WholeCollection)[i]);
                    }
                }

                //Elements supprimés
                for (int i = 0; i < DeletedItems.Count; i++)
                {
                    if (((BaseObject)DeletedItems[i]).IsPersistent)
                    {
                        ((BaseObject)DeletedItems[i]).ObjectState = BaseObject.ObjectStateType.Deleted;
                        ((CustomCollection)Rt).Add(DeletedItems[i]);
                    }
                }

                ((CustomCollection)Rt).IsChangesContainer = true;
                return (CustomCollection)Rt;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private CustomCollection deleteditems; // clone de la Collection Cre juste aprs le chargement de la base de donnes
        public CustomCollection DeletedItems
        {
            get
            {
                if (deleteditems == null)
                {
                    deleteditems = (CustomCollection)Activator.CreateInstance(this.GetType());
                }
                return deleteditems;
            }
            set
            {
                deleteditems = value;
            }
        }
        private string filter = string.Empty;
        internal CustomCollection filtreditems; // Collection contenant les lments filtrs (Supprims);
        public CustomCollection FiltredItems
        {
            get
            {
                if (filtreditems == null)
                {
                    filtreditems = (CustomCollection)Activator.CreateInstance(this.GetType());
                }
                return filtreditems;
            }
            set
            {
                filtreditems = value;
            }

        }
        public string ItemFilter
        {
            get
            {
                return filter;
            }
            set
            {
                try
                {
                    if (value == filter) return;
                    this.IsFiltering = true;
                    //2- restauration de la collection   l'etat d'origine 
                    for (int i = 0; i <= this.FiltredItems.Count - 1; i++)
                    {
                        this.Add(this.FiltredItems[i]);
                    }

                    //Reset de la liste des lements filtrs
                    this.FiltredItems = (CustomCollection)Activator.CreateInstance(this.GetType());

                    //3- application du filtre si non vide
                    if (value != null & value != string.Empty)
                    {
                        Filter MyFilter = new Filter(this, value);
                    }
                    filter = value;

                    //4-Restauration du rafraichissement auto
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0, 0));
                    //ListChanged += onListChanged;

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region IList Members

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object this[int index]
        {
            get
            {
                return InnerList[index];
            }
            set
            {
                InnerList[index] = value;
            }
        }

        public new void RemoveAt(int index)
        {
            this.Remove(this[index]);
        }

        public void Insert(int index, object value)
        {
            ((BaseObject)value).Parent = this;
            InnerList.Insert(index, value);
        }

        public void Remove(object value)
        {
            this.DeletedItems.Add(value);
            int index = IndexOf(value);
            BaseObject obj = (BaseObject)value;
            obj.Parent = null;
            InnerList.Remove(value);
            OnItemDeleted(new System.EventArgs(), index);
            if (!this.IsFiltering)
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));

        }

        public bool Contains(object value)
        {
            return InnerList.Contains(value);
        }

        public new void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this.Remove(this[i]);
            }
        }

        public int IndexOf(object value)
        {
            return InnerList.IndexOf(value);
        }

        public int Add(object value)
        {
            ((BaseObject)value).Parent = this;
            int i = InnerList.Add(value);
            if (!this.IsFiltering)
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, IndexOf(value)));
            return i;
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        #endregion
        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public new int Count
        {
            get
            {
                return InnerList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            InnerList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return InnerList.SyncRoot;
            }
        }

        #endregion
        #region IEnumerable Members

        public new IEnumerator GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        #endregion$(-è_
        #region Collection Base
        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {

            BaseObject obj = (BaseObject)value;
            obj.Parent = this;
            if (!this.IsFiltering)
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                BaseObject oldobj = (BaseObject)oldValue;
                BaseObject newobj = (BaseObject)newValue;

                oldobj.Parent = null;
                newobj.Parent = this;
                if (!this.IsFiltering)
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }
        #endregion
        #region IBindingList Members

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }


        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return true; }
        }

        object IBindingList.AddNew()
        {
            object obj = Activator.CreateInstance(this.ItemType);
            this.Add(obj);
            return obj;
        }

        private bool isSorted = false;

        bool IBindingList.IsSorted
        {
            get { return isSorted; }
        }

        private ListSortDirection listSortDirection = ListSortDirection.Ascending;

        ListSortDirection IBindingList.SortDirection
        {
            get { return listSortDirection; }
        }

        PropertyDescriptor sortProperty = null;

        PropertyDescriptor IBindingList.SortProperty
        {
            get { return sortProperty; }
        }

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }
        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            isSorted = true;
            sortProperty = property;
            listSortDirection = direction;

            ArrayList a = new ArrayList();

            base.InnerList.Sort(new ObjectPropertyComparer(property.Name));
            if (direction == ListSortDirection.Descending) base.InnerList.Reverse();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            isSorted = false;
            sortProperty = null;
        }
        #endregion
        #region ListChanged Events
        internal ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;
        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }
        internal void CollectionChanged(object obj)
        {
            int index = List.IndexOf(obj);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }
        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }
        #endregion
        #region events
        private EventHandler _ItemDeleted;
        public event EventHandler ItemDeleted
        {
            add
            {
                _ItemDeleted += value;
            }
            remove
            {
                _ItemDeleted -= value;
            }
        }
        protected virtual void OnItemDeleted(EventArgs args, int index)
        {
            if (_ItemDeleted != null)
            {
                _ItemDeleted(this, args);
            }
        }
        #endregion

    }
}