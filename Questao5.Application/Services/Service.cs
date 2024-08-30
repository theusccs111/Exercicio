using Questao5.Application.Interfaces.Persistance;

namespace Questao5.Application.Services
{
    public class Service
    {
        
        private readonly IUnitOfWork _unitOfWork;
        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
