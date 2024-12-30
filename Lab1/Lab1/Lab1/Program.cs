using System.Numerics;

public enum DischargeReason
{
    TreatmentCompleted,
    NonCompliance,
    Other
}


public abstract class Person
{
    public string Name { get; set; }
    public int ID { get; set; }
}


public class Patient : Person
{
    public Doctor AssignedDoctor { get; set; }
    public List<Treatment> TreatmentHistory { get; private set; } = new List<Treatment>();
    public string Status { get; set; }

    public void AddTreatment(Treatment treatment)
    {
        TreatmentHistory.Add(treatment);
        Console.WriteLine($"Treatment '{treatment.Description}' added to patient {Name}.");
    }

    public void Discharge(DischargeReason reason)
    {
        switch (reason)
        {
            case DischargeReason.TreatmentCompleted:
                Status = "Discharged (Treatment Completed)";
                break;
            case DischargeReason.NonCompliance:
                Status = "Discharged (Non-Compliance)";
                break;
            case DischargeReason.Other:
                Status = "Discharged (Other)";
                break;
        }
        Console.WriteLine($"Patient {Name} has been discharged. Reason: {Status}");
    }
}


public class Doctor : Person
{
    public string Specialization { get; set; }
    private List<Patient> AssignedPatients { get; set; } = new List<Patient>();

    public void AssignTreatment(Patient patient, Treatment treatment)
    {
        patient.AddTreatment(treatment);
        treatment.AssignedStaff = this;
        Console.WriteLine($"Doctor {Name} assigned treatment '{treatment.Description}' to patient {patient.Name}.");
    }

    public void PerformTreatment(Treatment treatment)
    {
        if (treatment.Status == "Assigned")
        {
            treatment.MarkComplete();
            Console.WriteLine($"Doctor {Name} completed treatment '{treatment.Description}' for patient.");
        }
        else
        {
            Console.WriteLine($"Treatment '{treatment.Description}' is not ready to be completed or already completed.");
        }
    }
}


public class Nurse : Person
{
    public void PerformTreatment(Treatment treatment)
    {
        if (treatment.Status == "Assigned")
        {
            treatment.MarkComplete();
            Console.WriteLine($"Nurse {Name} completed treatment '{treatment.Description}'.");
        }
        else
        {
            Console.WriteLine($"Treatment '{treatment.Description}' is not ready to be completed.");
        }
    }
}


public class Treatment
{
    public int TreatmentID { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } = "Assigned";
    public Person AssignedStaff { get; set; }

    public void MarkComplete()
    {
        Status = "Completed";
    }
}


public class HospitalSystem
{
    public static void Main(string[] args)
    {
        // Создаем врача, пациента и медсестру
        Doctor doctor = new Doctor { Name = "Dr. Smith", ID = 1, Specialization = "Therapist" };
        Patient patient = new Patient { Name = "John Doe", ID = 100, AssignedDoctor = doctor };
        Nurse nurse = new Nurse { Name = "Nurse Emily", ID = 200 };

        // Создаем лечение и назначаем его пациенту
        Treatment treatment = new Treatment { TreatmentID = 123, Description = "Physical Therapy" };
        doctor.AssignTreatment(patient, treatment);

        // Медсестра выполняет лечение
        nurse.PerformTreatment(treatment);

        // Выписываем пациента после завершения лечения
        if (treatment.Status == "Completed")
        {
            patient.Discharge(DischargeReason.TreatmentCompleted);
        }

        // Демонстрация выписки по другим причинам
        Patient anotherPatient = new Patient { Name = "Jane Doe", ID = 101, AssignedDoctor = doctor };
        anotherPatient.Discharge(DischargeReason.NonCompliance); // Нарушение режима
        anotherPatient.Discharge(DischargeReason.Other);        // Иные обстоятельства
    }
}

