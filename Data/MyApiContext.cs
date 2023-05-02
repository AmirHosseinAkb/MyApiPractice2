using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MyApiContext:DbContext
    {
        public MyApiContext(DbContextOptions<MyApiContext> options):base(options)
        {
            
        }


    }
}
