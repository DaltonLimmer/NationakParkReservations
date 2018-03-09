--Select campground.name, site.site_id, reservation.name, reservation.from_date, reservation.to_date
--From campground 
--JOIN site ON site.campground_id = campground.campground_id
--JOIN reservation ON reservation.site_id = site.site_id

--SELECT * FROM campground JOIN park ON park.park_id = campground.park_id WHERE  park.name = @parkName;

--SELECT * From campground JOIN park ON park.park_id = campground.park_id

--SELECT * FROM campground
--JOIN site ON site.campground_id = campground.campground_id
--JOIN reservation ON reservation.site_id = site.site_id
--WHERE ('2018-03-10' < reservation.from_date AND @EndDate <= reservation.to_date) OR (@StartDate >= reservation.to_date AND @EndDate > reservation.to_date)

SELECT * FROM campground
LEFT JOIN site ON site.campground_id = campground.campground_id
LEFT JOIN reservation ON reservation.site_id = site.site_id
WHERE ('2018-03-10' > reservation.from_date AND '2018-03-10' < reservation.to_date) AND ('2018-03-14' > reservation.to_date AND'2018-03-14' > reservation.to_date)
