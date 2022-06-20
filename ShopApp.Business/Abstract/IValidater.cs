using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Abstract
{
    public interface IValidater<T>
    {
        string ErrorMessages { get; set; }
        bool Validate(T entity);

    }
}
