--select TOP 5 * from site where max_occupancy >= @numOfGuests and max_rv_length >= @rvlength and accessible = @wheelchairAccessible and utilities = @utilitiesHookup

--SELECT TOP 5 * FROM site join campground on campground.campground_id = site.campground_id " +
--            "WHERE campground.name = @campgroundName AND site.site_id NOT IN (SELECT site.site_id FROM reservation Join site ON site.site_id = reservation.site_id " +
--            "JOIN campground ON campground.campground_id = site.campground_id WHERE campground.name = @campgroundName and (@startDate between reservation.from_date and reservation.to_date) " +
--            "or (@endDate between reservation.from_date and reservation.to_date) or(reservation.from_date between @startDate and @endDate) or " +
--            "(reservation.to_date between @startDate and @endDate)  AND site.max_occupancy >= @numOfGuests and site.max_rv_length >= @rvlength and site.accessible = @wheelchairAccessible and site.utilities = @utilitiesHookup) order by site.site_id

SELECT * FROM site