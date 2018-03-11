----SELECT * from site 
----LEFT JOIN reservation ON reservation.site_id = site.site_id
----RIGHT Join campground On campground.campground_id = site.campground_id
----Join park on park.park_id = campground.park_id
----where park.name = 'Acadia'
----ORDER by site.site_id

----SELECT site.* FROM site
----JOIN campground ON campground.campground_id = site.campground_id
----JOIN park ON park.park_id = campground.park_id
----WHERE park.name = 'Acadia' AND site.site_id NOT IN (SELECT site.site_id FROM reservation join site ON reservation.site_id = site.site_id join campground ON campground.campground_id = site.campground_id 
----join park on campground.park_id = park.park_id 
----WHERE park.name = 'Acadia' AND  (('2018/03/11' between reservation.from_date and reservation.to_date) OR ('2018/03/15' between reservation.from_date and reservation.to_date)))

----SELECT * FROM site
----JOIN campground ON campground.campground_id = site.campground_id
----WHERE campground.name = 'Blackwoods'

--SELECT * FROM reservation
--Right JOIN site ON site.site_id = reservation.site_id
--JOIN campground ON campground.campground_id = site.campground_id
--WHERE campground.name = 'Blackwoods'

--SELECT * FROM site 
--join campground on campground.campground_id = site.campground_id
--WHERE campground.name = 'Blackwoods' AND site.site_id NOT IN (SELECT site.site_id FROM reservation Join site ON site.site_id = reservation.site_id JOIN campground ON campground.campground_id = site.campground_id
--WHERE campground.name = 'Blackwoods' and ('2018/03/03' between reservation.from_date and reservation.to_date) or 
--('2018/03/15' between reservation.from_date and reservation.to_date) or (reservation.from_date between '2018/03/03' and'2018/03/15') or 
--(reservation.to_date between '2018/03/03' and '2018/03/15')  ) order by site.site_id

--SELECT site.* FROM site
--JOIN campground ON campground.campground_id = site.campground_id
--JOIN park ON park.park_id = campground.park_id
--WHERE park.name = 'Acadia' AND site.site_id NOT IN (SELECT site.site_id FROM reservation join site ON reservation.site_id = site.site_id join campground ON campground.campground_id = site.campground_id 
--join park on campground.park_id = park.park_id 
--WHERE park.name = 'Acadia' AND  (('2018/03/11' between reservation.from_date and reservation.to_date) OR ('2018/03/15' between reservation.from_date and reservation.to_date)))

--SELECT site.site_id FROM reservation 
--Join site ON site.site_id = reservation.site_id 
--JOIN campground ON campground.campground_id = site.campground_id
--WHERE campground.name = 'Blackwoods' and (('2018/03/03' between reservation.from_date and reservation.to_date) or 
--('2018/03/15' between reservation.from_date and reservation.to_date))

SELECT * from reservation