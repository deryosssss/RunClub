import api from './Api'

export const getEvents = () => api.get('/events').then(res => res.data)
export const getEventById = (id) => api.get(`/events/${id}`).then(res => res.data)
export const createEvent = (eventData) => api.post('/events', eventData).then(res => res.data)
export const updateEvent = (id, data) => api.put(`/events/${id}`, data)
export const deleteEvent = (id) => api.delete(`/events/${id}`)
