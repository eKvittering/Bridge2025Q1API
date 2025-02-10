


namespace Webminux.Optician.Rooms
{
    /// <summary>
    /// DTO To Create Room Entity
    /// </summary>
    public class CreateRoomDto
    {
        /// <summary>
        /// Room Name 
        /// </summary>
        public virtual string Name{ get; set; }

        /// <summary>
        /// Additional information about room
        /// </summary>
        public virtual string Descriptions { get; set; }
    }
}