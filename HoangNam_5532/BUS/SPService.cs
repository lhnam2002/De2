using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SPService
    {
        public List<SanPham1> GetAll()
        {
            Model1 context = new Model1();
            return context.SanPham1.ToList();
        }

        public SanPham1 FindByID(string sanPhamID)
        {
            Model1 context = new Model1();
            return context.SanPham1.FirstOrDefault(p => p.MaSP == sanPhamID);
        }
    
    }
}
