namespace NZwalks.API.Models.Domain
{
    public class Difficulty
    {
        //when a unique reference number is needed to identify information on a computer or network-GUID(hard to duplicate)
        public Guid Id { get; set; }   //globally unique identifier

        public string Name { get; set; }
    }
}
