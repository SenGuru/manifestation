#pragma warning disable S2094 // Classes should not be empty
using System.Collections.Generic;
using Framework.Infrastructure.Exceptions;

namespace Manifest.Common.Models.ViewModels
{
    public class CanDeleteModel
    {
        public bool CanDelete { get; set; }

        public List<ReturnErrorItem> ReferencingTables { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty