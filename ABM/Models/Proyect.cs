//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Proyect
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proyect()
        {
            this.SuscriptorProyect = new HashSet<SuscriptorProyect>();
        }
    
        public int id { get; set; }
        public string proyectName { get; set; }
        public string proyectDetail { get; set; }
        public System.DateTime editDate { get; set; }
        public int StateId { get; set; }
        public int UserStateId { get; set; }
    
        public virtual ProyectState ProyectState { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuscriptorProyect> SuscriptorProyect { get; set; }
    }
}
