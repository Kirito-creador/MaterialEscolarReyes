namespace MaterialEscolarReyes.ViewModels
{
    public class CarritoItemViewModel
    {
        public int ProductoId { get; set; }

        public string Nombre { get; set; } = "";

        public decimal Precio { get; set; }

        public string? Imagen { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Precio * Cantidad;
            }
        }
    }
}