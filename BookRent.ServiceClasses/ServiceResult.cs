using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRent.ServiceClasses
{
    public class ServiceResult<T>
    {
        public bool BadRequest {  get; set; }
        public bool NotFound {  get; set; }
        public bool Ok { get; set; }
        public T Result { get; set; }
        public ServiceErrors Errors { get; set; }
    }
}
