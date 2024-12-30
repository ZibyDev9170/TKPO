using System.Collections.Generic;

namespace Lab5_Form
{
    public partial class Form1 : Form
    {
        private Lift lift;

        public Form1()
        {
            InitializeComponent();
            lift = new Lift(1000); // Грузоподъемность 1000 кг
            UpdateLiftStatus();
        }

        private void UpdateLiftStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateLiftStatus));
                return;
            }

            lblStatus.Text = $"Текущий этаж: {lift.CurrentFloor}\n" +
                             $"Текущая загрузка: {lift.CurrentLoad} кг\n" +
                             $"Состояние: {lift.CurrentStateName}";
        }

        private async void btnCallLift_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTargetFloor.Text, out int floor))
            {
                await lift.CallToFloor(floor, UpdateLiftStatus);
            }
            else
            {
                MessageBox.Show("Введите корректный номер этажа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoadCargo_Click(object sender, EventArgs e)
        {
            Console.WriteLine(txtCargoWeight.Text);
            if (double.TryParse(txtCargoWeight.Text, out double weight))
            {
                lift.LoadCargo(weight);
                UpdateLiftStatus();
            }
            else
            {
                MessageBox.Show("Введите корректный вес груза!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUnloadCargo_Click(object sender, EventArgs e)
        {
            Console.WriteLine(txtCargoWeight.Text);
            if (double.TryParse(txtCargoWeight.Text, out double weight))
            {
                lift.UnloadCargo(weight);
                UpdateLiftStatus();
            }
            else
            {
                MessageBox.Show("Введите корректный вес груза!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRestorePower_Click(object sender, EventArgs e)
        {
            lift.RestorePower();
            UpdateLiftStatus();
        }
    }
}
