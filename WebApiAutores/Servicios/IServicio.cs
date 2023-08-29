namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSinglenton();
        Guid ObtenerTransient();
        void RealizarTarea();

    }
    public class ServicioA : IServicio 
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;

        public ServicioA(ILogger<ServicioA> logger, ServicioTransient servicioTransient, ServicioScoped servicioScoped, ServicioSingleton servicioSingleton)
        {
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;

            //this.logger = Logger;
        }


        public Guid ObtenerTransient() { return servicioTransient.Guid; }
        public Guid ObtenerScoped() { return servicioScoped.Guid; }
        public Guid ObtenerSinglenton() { return servicioSingleton.Guid; }


        public void RealizarTarea() 
        {
            throw new NotImplementedException();
        }
    }

    public class ServicioB: IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSinglenton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {
            throw new NotSupportedException();
        }
    }

    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid(); //string aleatorio
    }

    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid(); //string aleatorio
    }

    public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid(); //string aleatorio
    }



}
