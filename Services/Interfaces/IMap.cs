namespace Services.Interfaces
{
    internal interface IMap<TEntity, TDto>
    {
        TEntity Map(TDto dto);
        TDto Map(TEntity entity);
    }
}
