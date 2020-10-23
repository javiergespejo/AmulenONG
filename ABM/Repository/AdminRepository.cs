using ABM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace ABM.Repository
{
    public class AdminRepository : GenericRepository<MercadoPagoButton>, IDisposable
    {
        private bool _disposed = false;

        public AdminRepository(AmulenEntities context) : base(context)
        {

        }
        /// <summary>
        /// Inserts a new MercadoPagoButton to the database
        /// </summary>
        /// <param name="mpButton"></param>
        public void InsertDonationButton(MercadoPagoButton mpButton)
        {
            base.context.MercadoPagoButton.Add(mpButton);
            Save();
        }
        /// <summary>
        /// Gets the list of DonationButtons
        /// </summary>
        /// <returns>List of DonationButtons</returns>
        public IEnumerable<MercadoPagoButton> GetDonationButtonList()
        {
            return this.Get();
        }

        /// <summary>
        /// Updates a MercadoPagoButton in the database
        /// </summary>
        /// <param name="mpButton"></param>
        public void UpdateDonationButton(MercadoPagoButton mpButton)
        {
            base.Update(mpButton);
            Save();
        }
        /// <summary>
        /// Remove a MercadoPagoButton in the database
        /// </summary>
        /// <param name="id"></param>
        public void RemoveDonationButton(int id)
        {
            base.Delete(id);
            Save();
        }
        public void Save()
        {
            base.context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    base.context.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}