select a.Name,b.AppointmentDate from
Patients a
inner join Appointments b
on a.PatientID=b.PatientID
where b.AppointmentDate>= GETDATE()- 30