using Castle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfDecorators
{
	/// <summary>Интерфейс сущности, имеющей дату последнего изменения</summary>
	public interface IHasLastChangeDate : IEntity
	{
		/// <summary>Дата последнего изменения</summary>
		DateTime? LastChangeDate { get; set; }
	}

	/// <summary>Декоратор заполнения данных о дате и времени последнего изменения при создании сущности</summary>
	internal interface ISetLastChangeDateCreateDecorator<TEntity> : ICreateDecorator<TEntity>
		where TEntity : class, IHasLastChangeDate
	{
	}

	/// <summary>
	/// Декоратор по установке автора последнего изменения
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	[RegisterDecorator(typeof(ISetLastChangeDateCreateDecorator<>))]
	internal sealed class SetLastChangeDateCreateDecorator<TEntity> : ISetLastChangeDateCreateDecorator<TEntity>
	where TEntity : class, IHasLastChangeDate
	{
		[DoNotWire]
		public ICreateDecorator<TEntity> CreateDecorator { get; set; }

		public IReadOnlyList<TEntity> Create(IReadOnlyList<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				entity.LastChangeDate = DateTime.Now;
			}

			return CreateDecorator.Create(entities);
		}
	}
}
