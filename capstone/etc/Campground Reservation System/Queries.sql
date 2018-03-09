--SELECT site.id from site, reservation
--WHERE (@reservStart > reservation.from_date) AND (@reservStart < reservation.to_date) ||

--SELECT reservation.*, campground.name FROM reservation
--JOIN site ON site.site_id = reservation.site_id
--JOIN campground ON campground.campground_id = site.campground_id
--ORDER BY reservation.site_id asc

--SELECT *
--FROM site
--WHERE site.site_id NOT IN (
--SELECT site_id FROM reservation
--WHERE ('2018/03/01' BETWEEN  reservation.from_date AND reservation.to_date) OR
--('2018/03/31' BETWEEN  reservation.from_date AND reservation.to_date))
-- ORDER BY site.site_id

--SELECT * from reservation
--ORDER BY reservation.from_date, reservation.to_date



