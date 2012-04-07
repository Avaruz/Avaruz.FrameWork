using System;
using System.Collections;

namespace Avaruz.FrameWork.DAF
{
    /// <summary>
    /// 
    /// </summary>
	public class SentencesClass : DictionaryBase
	{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public String this[String key]
		{
			get
			{
				return ((String)Dictionary[key]);
			}
			set
			{
				Dictionary[key] = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public ICollection Keys
		{
			get
			{
				return (Dictionary.Keys);
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public ICollection Values
		{
			get
			{
				return (Dictionary.Values);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(String key, String value)
		{
			Dictionary.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public bool Contains(String key)
		{
			return (Dictionary.Contains(key));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
		public void Remove(String key)
		{
			Dictionary.Remove(key);
		}

	}
}
