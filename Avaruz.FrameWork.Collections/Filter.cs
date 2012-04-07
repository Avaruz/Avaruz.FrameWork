using System;
using System.Reflection;
using System.Collections;
namespace Avaruz.FrameWork.Collections
{
	/// <summary>
	/// Summary description for Filter.
	/// </summary>
	public class Expressions : CollectionBase

	{
		public Expressions (string HoleFilterExpression)
		{
			try
			{
				this.SplitFilter(HoleFilterExpression);
			}
			catch(System.Exception ex)
			{
				throw ex;
			}
		}
		public Expressions() 
		{

		}
		public void SplitFilter(string HoleFilterExpression)
		{
			try
			{
				int LastPosition = 0;
			
				//AND
				for (int j = 5; j <= HoleFilterExpression.Length -5; j ++)
				{
				
					string FiveCurrentChars = HoleFilterExpression.Substring(j-5, 5).ToUpper();
					if (FiveCurrentChars == " AND ") 
					{
						
						
						this.Add( new Expression(HoleFilterExpression.Substring(LastPosition , j - LastPosition -5 )));
						LastPosition = j;
					}
				}
				//OR
				for (int z = 4; z <= HoleFilterExpression.Length -4; z ++)
				{
					string TowCurrentChars = HoleFilterExpression.Substring(z -4, 4).ToUpper();
					if (TowCurrentChars == " OR " ) 
					{
						this.Add(new Expression(HoleFilterExpression.Substring(LastPosition  , z - LastPosition -4)));
						LastPosition = z;
					}
				}
			
				//Ajouter le dernier élement ou le premier si aucun AND/OR
				this.Add(new Expression(HoleFilterExpression.Substring(LastPosition)));
			}
			catch(System.Exception ex)
			{
				throw ex;
			}
		}
		public Expression Item(int Index)
		{
			return (Expression) List[Index];
					
		}
		
		public int Add (Expression value) 
		{
			
			return List.Add(value);
		}

		

		public void Remove (Expression value) 
		{
		
			List.Remove(value);
		}

		internal void ElementChanged(Expression cust) 
		{
        
			int index = List.IndexOf(cust);
        
			
		}
	}
	public class Expression
	{
		private string TmpPropertyValue;
		private string TmpOperator;
		private string TmpUserValue;
		public Expression()
		{
		}
		public Expression(string PropValue, string Opr, string Usrvalue )
		{
			this.PropertyName = PropValue;
			this.Operator = Opr;
			this.UserValue = this.UserValue = Usrvalue;
		}
		public Expression( string HoleExpression)
		{
			try
			{
				string [] Words = new string[2] ;
				Words = HoleExpression.Split(new char[1]{' '}, 3);
				this.PropertyName = Words[0];
				this.Operator = Words[1].Trim();
				this.UserValue = Words[2].Trim ();
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		public string PropertyName
		{
			get{return TmpPropertyValue;}
			set{TmpPropertyValue = value;}
		}
		public string Operator
		{
			get{return TmpOperator;}
			set{TmpOperator = value;}
		}
		public string UserValue
		{
			get{return TmpUserValue;}
			set{TmpUserValue = value;}
		}
	}
	
	public class Filter
	{
		
		public Filter()
		{
		}
		public Filter(object ObjToFilter, string Filter)
		{
			
			this.ApplyFilter(ObjToFilter, Filter);
		}
		private bool IsOk(string ObjectPropertyValue , string Operator, string UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue.TrimEnd() == UserValue.TrimEnd()) {Rt = true;}
				}
				else if(Operator == "<>" |Operator == "!=")
				{
					if (ObjectPropertyValue.TrimEnd() != UserValue.TrimEnd()) {Rt = true;}
				}
				else if (Operator.ToUpper() == "LIKE")
				{
					//recherche des étoiles
					int LastStrar = UserValue.LastIndexOf("*");
					int UserValueLenght = UserValue.Length ;
					string SearchedText = UserValue.Replace("*", "");
					int TextPosition = ObjectPropertyValue.IndexOf(SearchedText);
					if (TextPosition != -1)
					{
					{Rt = true;}
					}
				}
				else
				{
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type string !");
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(int ObjectPropertyValue , string Operator, int UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else if (Operator == ">")
				{
					if (ObjectPropertyValue > UserValue) {Rt = true;}
				}
				else if (Operator == ">=")
				{
					if (ObjectPropertyValue >= UserValue) {Rt = true;}
				}
				else if (Operator == "<")
				{
					if (ObjectPropertyValue < UserValue) {Rt = true;}
				}
				else if (Operator == "<=")
				{
					if (ObjectPropertyValue <= UserValue) {Rt = true;}
				}
				
				else if(Operator == "<>"|Operator == "!=")
				{
					if (ObjectPropertyValue != UserValue) {Rt = true;}
				}

				else
				{
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type int !");
				}
				return Rt;
				
			}
			catch (System.Exception ex)
			{throw ex;}
		}

		private bool IsOk(decimal ObjectPropertyValue , string Operator, int UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else if (Operator == ">")
				{
					if (ObjectPropertyValue > UserValue) {Rt = true;}
				}
				else if (Operator == ">=")
				{
					if (ObjectPropertyValue >= UserValue) {Rt = true;}
				}
				else if (Operator == "<")
				{
					if (ObjectPropertyValue < UserValue) {Rt = true;}
				}
				else if (Operator == "<=")
				{
					if (ObjectPropertyValue <= UserValue) {Rt = true;}
				}
				
				else if(Operator == "<>"|Operator == "!=")
				{
					if (ObjectPropertyValue != UserValue) {Rt = true;}
				}

				else
				{
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type int !");
				}
				return Rt;
				
			}
			catch (System.Exception ex)
			{throw ex;}
		}

		private bool IsOk(Guid ObjectPropertyValue , string Operator, Guid UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else
				{
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type Guid !");
				}
				return Rt;
				
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(double ObjectPropertyValue , string Operator, double UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else if (Operator == ">")
				{
					if (ObjectPropertyValue > UserValue) {Rt = true;}
				}
				else if (Operator == ">=")
				{
					if (ObjectPropertyValue >= UserValue) {Rt = true;}
				}
				else if (Operator == "<")
				{
					if (ObjectPropertyValue < UserValue) {Rt = true;}
				}
				else if (Operator == "<=")
				{
					if (ObjectPropertyValue <= UserValue) {Rt = true;}
				}
				else
				{
					
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type double !");
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(decimal ObjectPropertyValue , string Operator, decimal UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else if (Operator == ">")
				{
					if (ObjectPropertyValue > UserValue) {Rt = true;}
				}
				else if (Operator == ">=")
				{
					if (ObjectPropertyValue >= UserValue) {Rt = true;}
				}
				else if (Operator == "<")
				{
					if (ObjectPropertyValue < UserValue) {Rt = true;}
				}
				else if (Operator == "<=")
				{
					if (ObjectPropertyValue <= UserValue) {Rt = true;}
				}
				else
				{
					
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type decimal !");
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(DateTime ObjectPropertyValue , string Operator, DateTime UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else if (Operator == ">")
				{
					if (ObjectPropertyValue > UserValue) {Rt = true;}
				}
				else if (Operator == ">=")
				{
					if (ObjectPropertyValue >= UserValue) {Rt = true;}
				}
				else if (Operator == "<")
				{
					if (ObjectPropertyValue < UserValue) {Rt = true;}
				}
				else if (Operator == "<=")
				{
					if (ObjectPropertyValue <= UserValue) {Rt = true;}
				}
				else
				{
					
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type DateTime !");
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(bool ObjectPropertyValue , string Operator, bool UserValue)
		{
			bool Rt = false;
			try
			{
				if (Operator == "=")
				{
					if (ObjectPropertyValue == UserValue) {Rt = true;}
				}
				else
				{
					throw new Exception("L'opérateur '" + Operator + "' n'est pas géré pour le type string !");
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private bool IsOk(string Operator, object UserValue)
		{
			bool Rt = false;
			try
			{
				if (UserValue.ToString().ToUpper() == "NULL" & Operator == "=" )
				{
					Rt = true;
				}
				return Rt;
			}
			catch (System.Exception ex)
			{throw ex;}
		}
		private string CorrectUserValue(string UserValue)
		{
			try
			{
				if (UserValue.Substring(0,1)  == "'")
				{
					UserValue =UserValue.Replace("'", "");
				}
				
				if (UserValue.Substring(0,1)  == "#")
				{
					UserValue =UserValue.Replace("#", "");
				}

				return UserValue;
			}
			catch(System.Exception ex)
			{
				throw ex;
			}
		}
		private bool IsOk(object ObjectPropertyValue , string Operator, string UserValue)
		{

			bool Rt = false ;
			try
			{
				//Type of the property value
				if (ObjectPropertyValue == null)
				{
				}
				Type TypeOfValue = ObjectPropertyValue.GetType();
				//Object FilterValue  = Convert.ChangeType(CorrectUserValue(UserValue),TypeOfValue) ;
				Object FilterValue  = CorrectUserValue(UserValue) ;
				
				if (TypeOfValue ==typeof(string))
				{Rt = IsOk((string)ObjectPropertyValue, Operator, (string) FilterValue);}
				
				else if (TypeOfValue ==typeof(int))
				{Rt = IsOk((int)ObjectPropertyValue, Operator, (int) Convert.ToInt32(FilterValue));}
				
				else if (TypeOfValue == typeof(double))
				{Rt = IsOk((double)ObjectPropertyValue, Operator, (double)Convert.ToDouble(FilterValue));}
				
				else if (TypeOfValue == typeof(decimal))
				{Rt = IsOk((decimal)ObjectPropertyValue, Operator, (decimal)Convert.ToDecimal( FilterValue));}
				
				else if (TypeOfValue == typeof(DateTime))
				{Rt = IsOk((DateTime)ObjectPropertyValue, Operator, (DateTime) Convert.ToDateTime(FilterValue));}
				
				else if (TypeOfValue == typeof(bool))
				{Rt = IsOk((bool)ObjectPropertyValue, Operator, (bool) Convert.ToBoolean (FilterValue));}
				
				else if (TypeOfValue == typeof(Guid))
				{Rt = IsOk((Guid)ObjectPropertyValue, Operator, new Guid(FilterValue.ToString()));}
				else if (TypeOfValue == typeof(decimal))
				{
				{Rt = IsOk((decimal)ObjectPropertyValue, Operator, (decimal) Convert.ToDecimal(FilterValue));}
				}
				else
				{throw new Exception("Filtrage impossible sur le Type " + TypeOfValue.ToString());}
				return Rt;
			}
			catch(System.Exception ex)
			{
				throw ex;
			}
			
			
		}
		public void ApplyFilter(object objToFilter, string StrFilter)
		{
			try
			{
				DateTime D1 = DateTime.Now ;

				CustomCollection ObjectToFilter = (CustomCollection)objToFilter;
				int CountValue = ObjectToFilter.Count;
				Expressions ListOfExpressions = new Expressions(StrFilter);


				//Loading items of the collection
				bool [] Validations;
				bool AllIsOK;
				
				PropertyInfo [] PropsInfo = new PropertyInfo[ListOfExpressions.Count ];
				for(int x = 0; x< ListOfExpressions.Count;x++)
				{
					PropsInfo	[x] = ObjectToFilter.ItemType.GetProperty (ListOfExpressions.Item(x).PropertyName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
					if( PropsInfo[x] == null) 
					{
						throw new Exception ("property " + ListOfExpressions.Item(x).PropertyName + " does not exists!");
					}
				}
				
				for (int f = 0; f<= (ObjectToFilter.Count -1); f++)
				{
					object CollectionItem = ObjectToFilter[f];
					Type TypeOfItem = CollectionItem.GetType();

					Validations = new bool[ListOfExpressions.Count ];
					AllIsOK = true;

					for (int t = 0 ; t<= ListOfExpressions.Count -1 ; t++)
					{
						PropertyInfo ItemProperty = PropsInfo[t];
						object PropertyValue = ItemProperty.GetValue(CollectionItem, new object[0]);

						if (PropertyValue ==null)
						{
							Validations[t] = IsOk(ListOfExpressions.Item(t).Operator , ListOfExpressions.Item(t).UserValue );
						}
						else
						{
							Validations[t] = this.IsOk(PropertyValue, ListOfExpressions.Item(t).Operator , ListOfExpressions.Item(t).UserValue );
						}
						if (Validations[t] == false)
						{AllIsOK = false;}
						//System.Diagnostics.Trace.WriteLine("time : " + (DateTime.Now - timeSt1).TotalMilliseconds.ToString() + " cicle : " + cicle.ToString());
						//timeSt1 = DateTime.Now;
						//cicle++;
					}

					if (AllIsOK == false)
					{
						ObjectToFilter.Remove(CollectionItem);
						ObjectToFilter.filtreditems.Add(CollectionItem);
						CountValue -= 1;
						f -= 1;
					}
				}
		
				DateTime D2= DateTime.Now ;
				Console.WriteLine((D2-D1).ToString());
			
	
			}
			catch(System.Exception ex)
			{
				throw ex;
			}

		}
	}

}
