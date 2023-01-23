// See https://aka.ms/new-console-template for more information
using Castle.Windsor;

Console.WriteLine("Hello, World!");

public interface IEntity
{
	object Id { get; set; }
}

public class DecorationServiceAttribute : Attribute
{
	internal Type Service { get; }
	internal Type MainImpl { get; }
	internal Type[] Decorators { get; }

	public DecorationServiceAttribute(Type service, Type mainImpl, params Type[] decorators)
	{
		Service = service;
		MainImpl = mainImpl;
		Decorators = decorators;
	}
}

public class RegisterDecoratorAttribute : RegisterTransientAttribute
{
	public RegisterDecoratorAttribute(params Type[] types) : base(types)
	{
	}
}

/// <summary>Атрибут регистрации синглтона</summary>
[AttributeUsage(AttributeTargets.Class)]
public class RegisterTransientAttribute : RegisterDependencyAttribute
{
	public RegisterTransientAttribute(params Type[] services) : base(EDependencyLifestyle.Transient, services)
	{
	}
}

/// <summary>Приоритет реализации зависимости</summary>
public enum EImplementationPrecedence
{
	/// <summary>Fallback</summary>
	Fallback,

	/// <summary>Обычный</summary>
	Normal,

	/// <summary>Default</summary>
	Default
}

/// <summary>Лайфстайлы</summary>
public enum EDependencyLifestyle
{
	/// <summary>Синглтон</summary>
	Singleton,

	/// <summary>Транзиент</summary>
	Transient
}

/// <summary>Регистратор компонента</summary>
public interface IRegistrator
{
	/// <summary>Зарегистрировать</summary>
	/// <param name="container">Контейнер</param>
	/// <param name="implementation">Реализация</param>
	void Register(IWindsorContainer container, Type implementation);
}

/// <summary>
/// Интерфейс определения аттрибутов для регистрации
/// </summary>
public interface IRegisterDependencyAttribute
{
	/// <summary>
	/// Регистратор компонента
	/// </summary>
	IRegistrator Registrator { get; }
}

/// <summary>Атрибут регистрации зависимости</summary>
public abstract class RegisterDependencyAttribute : Attribute, IRegisterDependencyAttribute
{
	private readonly Lazy<RegistratorImpl> _registrator;

	public virtual IRegistrator Registrator => _registrator.Value;

	/// <summary>Приоритет реализации зависимости</summary>
	public EImplementationPrecedence Precedence { get; set; } = EImplementationPrecedence.Normal;

	public RegisterDependencyAttribute(EDependencyLifestyle lifestyle, params Type[] services)
	{
		_registrator = new Lazy<RegistratorImpl>(() => new RegistratorImpl(services, lifestyle, Precedence));
	}

	private class RegistratorImpl : IRegistrator
	{
		private readonly EImplementationPrecedence _precedence;

		private readonly EDependencyLifestyle _lifestyle;

		private readonly Type[] _services;

		public RegistratorImpl(Type[] services, EDependencyLifestyle lifestyle, EImplementationPrecedence precedence)
		{
			_services = services;
			_lifestyle = lifestyle;
			_precedence = precedence;
		}

		public void Register(IWindsorContainer container, Type implementation)
		{
			var component = Castle.MicroKernel.Registration.Component.For(_services).ImplementedBy(implementation);
			container.Register(component);
		}
	}
}

internal interface ICreateDecorator<TEntity>
where TEntity : class, IEntity
{
	ICreateDecorator<TEntity> CreateDecorator { get; set; }

	IReadOnlyList<TEntity> Create(IReadOnlyList<TEntity> entities);
}