--select TOP 5 * from site where max_occupancy >= @numOfGuests and max_rv_length >= @rvlength and accessible = @wheelchairAccessible and utilities = @utilitiesHookup

SELECT TOP 5 * FROM site 
join campground on campground.campground_id = site.campground_id 
WHERE campground.name = 'Seawall'
AND site.max_occupancy >= 3 and site.max_rv_length >= 2 
	and site.accessible = (1) and site.utilities = (1) AND site.site_id NOT IN 
(SELECT site.site_id FROM reservation Join site ON site.site_id = reservation.site_id 
	JOIN campground ON campground.campground_id = site.campground_id 
	WHERE campground.name = 'Seawall' and ('04/05/2018' between reservation.from_date and reservation.to_date) 
	or ('04/08/2018' between reservation.from_date and reservation.to_date) or(reservation.from_date between '04/05/2018' and '04/08/2018') 
	or (reservation.to_date between '04/05/2018' and '04/08/2018')  AND site.max_occupancy >= 3 and site.max_rv_length >= 2 
	and site.accessible = (1) and site.utilities = (1)) order by site.site_id

--SELECT * FROM campground
SELECT * FROM SITE JOIN campground ON campground.campground_id = site.campground_id WHERE campground.name = 'Seawall'
SELECT * FROM reservation
