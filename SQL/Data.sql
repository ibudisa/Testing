select y.*,'Highest' from
(
select x.shop_name,ROW_NUMBER() OVER(ORDER BY x.shopproductivity  desc) AS tl,x.shopproductivity 
from
(
select w.shop_name,sum(w.data) / sum( DATEDIFF(HOUR , w.StartTime,w.EndTime)) as shopproductivity

from(
select a.product_id,a.shift_id, c.price *a.quantity_sold as data,b.shop_id,e.shop_name,b.StartTime,b.EndTime from
Sale a
inner join workshift b
on a.shift_id=b.shift_id
inner join product c
on a.product_id=c.product_id
inner join salesperson d
on b.salesperson_id=d.salesperson_id
inner join shop e
on b.shop_id=e.shop_id
where b.Date>=GETDATE()-28
)w
group by w.shop_name
)x)y
where y.tl between 1 and 2
union all
select y.*,'Lowest' from
(
select x.shop_name,ROW_NUMBER() OVER(ORDER BY x.shopproductivity  asc) AS tl,x.shopproductivity 
from
(
select w.shop_name,sum(w.data) / sum( DATEDIFF(HOUR , w.StartTime,w.EndTime)) as shopproductivity

from(
select a.product_id,a.shift_id, c.price *a.quantity_sold as data,b.shop_id,e.shop_name,b.StartTime,b.EndTime from
Sale a
inner join workshift b
on a.shift_id=b.shift_id
inner join product c
on a.product_id=c.product_id
inner join salesperson d
on b.salesperson_id=d.salesperson_id
inner join shop e
on b.shop_id=e.shop_id
where b.Date>=GETDATE()-28
)w
group by w.shop_name
)x)y
where y.tl  between 1 and 2
