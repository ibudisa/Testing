
select a.name as son,b.name as father,c.name as mother from people a
inner join people B ON a.fatherId=b.id
inner join people c
on a.motherId=c.id
where a.age in (
select x.age from
(
select min(a.age) as age,b.name as father,c.name as mother from people a
inner join people b
on a.fatherId=b.id
inner join people c
on a.motherId=c.id
group by b.name,c.name
)x
)
