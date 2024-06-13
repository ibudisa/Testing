select x.Name,c.averageduration from Patients x
inner join
(select b.PatientID,AVG(b.Duration) as averageduration from
(
select a.* from
(
select ROW_NUMBER() OVER(PARTITION BY PatientID                                   
       ORDER BY AppointmentDate DESC) AS Num,
	   AppointmentID,
	   PatientID,
	   AppointmentDate,
	   Duration
from Appointments
)a
where a.Num between 1 and 5
)b
group by b.PatientID)c
on x.PatientID=c.PatientID