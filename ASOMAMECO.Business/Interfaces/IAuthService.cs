using ASOMAMECO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Business.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioDto> LoginAsync(LoginModel loginModel);
        
    }

}
