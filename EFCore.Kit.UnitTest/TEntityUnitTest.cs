using EFCore.Kit.SeedWork;
using System;
using Xunit;

namespace EFCore.Kit.UnitTest
{
    public class TEntityUnitTest
    {
        public class GuidEntity : TEntity<Guid>
        {
            public GuidEntity(Guid id)
            {
                Id = id;
            }
        }
        [Fact]
        public void Equalses()
        {
            Guid nid = Guid.NewGuid();
            GuidEntity a = new GuidEntity(nid);
            GuidEntity b = new GuidEntity(nid);
            Assert.True(a == b);
        }
    }
}
