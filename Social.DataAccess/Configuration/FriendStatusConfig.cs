using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Aggregates.FriendRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.DataAccess.Configuration
{
    internal class FriendStatusConfig : IEntityTypeConfiguration<FriendStatus>
    {
        public void Configure(EntityTypeBuilder<FriendStatus> builder)
        {
            builder.HasKey(status => status.FriendshipId);
        }
    }
}
