namespace PC.Store.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons", Consts.DbSchema);

        builder
            .HasIndex(x => new { x.Email })
            .IsUnique();

        builder.HasIndex(x => x.ChatId)
            .IsUnique();
        
        builder.Property(x => x.CurrentRegistrationStep)
            .HasConversion(new EnumToStringConverter<RegistrationSteps>());
    }
}