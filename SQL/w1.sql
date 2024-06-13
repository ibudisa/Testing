select b.name,count(*) from employees a
inner join departments b
on a.departmentId=b.id
group by b.name