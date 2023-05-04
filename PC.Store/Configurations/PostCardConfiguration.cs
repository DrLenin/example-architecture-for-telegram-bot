namespace PC.Store.Configurations;

public class PostCardConfiguration : IEntityTypeConfiguration<PostCard>
{
    public void Configure(EntityTypeBuilder<PostCard> builder)
    {
        builder.ToTable("postcards", Consts.DbSchema);
        
        builder.Property(x => x.Status)
            .HasConversion(new EnumToStringConverter<PostCardStatus>());

        builder.HasOne(x => x.Sender)
            .WithMany(x => x.SendingPostCards)
            .HasForeignKey(x => x.SenderId);
        
        builder.HasOne(x => x.Receiver)
            .WithMany(x => x.ReceivePostCards)
            .HasForeignKey(x => x.ReceiverId);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);
        
        builder.Property(o => o.Code)
            .HasDefaultValueSql($"nextval('{Consts.DbSchema}.\"{nameof(PostCard.Code)}\"')");
    }
}