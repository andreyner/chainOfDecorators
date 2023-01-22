using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfDecorators
{
	/// <summary>
	/// Сервис создания сущностей
	/// </summary>
	internal interface ICreateDataService<TEntity> : ICreateDecorator<TEntity>
		where TEntity : class, IEntity
	{
	}

	[DecorationService(
	typeof(ICreateDecorator<Order>),
	typeof(ISetLastChangeDateCreateDecorator<Order>))]
	internal interface IOrderCreateService : ICreateDataService<Order>
	{
	}
}
