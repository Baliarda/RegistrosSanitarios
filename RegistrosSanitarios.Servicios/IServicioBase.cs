using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrosSanitarios.Servicios
{
    public interface IServicioBase<T>
    {
        Task<IEnumerable<T>> TraerTodosHabilitados();

        Task<IEnumerable<T>> TraerTodos();

        Task<T> TraerPorId(int id);

        bool Exists(int id);

        Task Agregar(T p);

        Task Modificar(T p);

        Task Eliminar(int id);

        bool AsociadoAServicio(T p);
    }
}