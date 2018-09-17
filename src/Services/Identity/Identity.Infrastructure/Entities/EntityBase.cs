namespace Identity.Infrastructure.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     Provides a base class for your objects which will be persisted to the database.
    ///     Benefits include the addition of an Id property along with a consistent manner for comparing
    ///     entities.
    ///     Since nearly all of the entities you create will have a type of int Id, this
    ///     base class leverages this assumption.  If you want an entity with a type other
    ///     than int, such as string, then use <see cref="EntityBaseWithTypedId{TypeId}" /> instead.
    /// </summary>
    public abstract class EntityBase : EntityBaseWithTypedId<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get => base.Id; protected set => base.Id = value; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public string ModifiedBy { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name} Id={this.Id}";
        }

        public virtual IEntityTypeConfiguration<TEntity> OnCreateEntityTypeConfiguration<TEntity>()
            where TEntity : EntityBase
        {
            // If return NULL it will use default entity type configuration provided by framework
            return null;
        }
    }
}
