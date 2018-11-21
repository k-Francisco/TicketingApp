using SpevoCore.Services.Body_Creation_Service.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Services.BodyCreationService
{
    public static class BodyCreationService
    {
        public static IFieldBody Field { get { return new FieldImplementation(); } }
    }
}
