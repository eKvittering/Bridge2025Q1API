namespace Webminux.Optician.CommonDtos
{
    public class SingleValueDto<T>
    {
        public T Value { get; set; }

        public SingleValueDto(T input)
        {
            Value = input;
        }
    }
}
