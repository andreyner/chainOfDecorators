using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfDecorators
{
	internal class Order : IHasLastChangeDate
	{
		/// <summary>
		/// Дата последнего измененияия
		/// </summary>
		public DateTime? LastChangeDate { get; set; }

		/// <summary>
		/// Идентификатор заказа
		/// </summary>
		public object Id { get; set; }

		/// <summary>
		/// Покупатель
		/// </summary>
		public string Customer { get; set; }
	}
}
