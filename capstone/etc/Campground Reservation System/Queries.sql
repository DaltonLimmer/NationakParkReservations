
--SELECT * FROM campground
--SELECT * FROM SITE 
SELECT * FROM reservation ORDER BY reservation.from_date

--SELECT reservation.* FROM reservation
--JOIN site ON site.site_id = reservation.site_id
--JOIN campground ON campground.campground_id = site.campground_id
--JOIN park ON park.park_id = campground.park_id
--WHERE park.name = 'acadia' 
--AND ( reservation.from_date BETWEEN GETDATE() AND DATEADD(day, 30, GetDATE()) )
--ORDER BY reservation.from_date

--SELECT reservation.* FROM reservation
--JOIN site ON site.site_id = reservation.site_id
--JOIN campground ON campground.campground_id = site.campground_id
--JOIN park ON park.park_id = campground.park_id

SELECT TOP 5 * FROM site 
JOIN campground ON campground.campground_id = site.campground_id 
JOIN park ON park.park_id = campground.park_id 
WHERE park.name = 'Acadia' AND site.site_id NOT IN 
(SELECT site.site_id FROM reservation join site ON reservation.site_id = site.site_id 
join campground ON campground.campground_id = site.campground_id 
join park on campground.park_id = park.park_id WHERE park.name = 'Acadia' 
AND (('2018/06/10' between reservation.from_date and reservation.to_date) 
OR ('2018/06/14' between reservation.from_date and reservation.to_date)))

