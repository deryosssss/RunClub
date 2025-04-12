// src/pages/Public/MediaGalleryPage.jsx
import React, { useState } from 'react'
import Header from '../../components/Header'
import { Modal, Button, Form } from 'react-bootstrap'
import { useApp } from '../../context/AppContext'
import './MediaGalleryPage.css'

const initialImages = [
  'https://images.unsplash.com/photo-1610816529690-1c56b5919f22?q=80&w=3270&auto=format&fit=crop',
  'https://images.unsplash.com/photo-1731769698193-d4890f840d8d?q=80&w=3270&auto=format&fit=crop',
  'https://images.unsplash.com/photo-1602070429746-fbef1c923e7f?q=80&w=3270&auto=format&fit=crop',
  'https://plus.unsplash.com/premium_photo-1725905518200-b5a04d4faa90?q=80&w=3271&auto=format&fit=crop',
  'https://images.unsplash.com/photo-1719299246416-b4c069be9caf?q=80&w=3270&auto=format&fit=crop',
  'https://images.unsplash.com/photo-1452626038306-9aae5e071dd3?q=80&w=3274&auto=format&fit=crop',
]

const MediaGalleryPage = () => {
  const { user } = useApp()
  const isAdmin = user?.role?.toLowerCase() === 'admin'

  const [images, setImages] = useState(initialImages)
  const [selectedImage, setSelectedImage] = useState(null)
  const [showAddModal, setShowAddModal] = useState(false)
  const [newImageUrl, setNewImageUrl] = useState('')

  const handleImageClick = (src) => setSelectedImage(src)
  const handleCloseModal = () => setSelectedImage(null)

  const handleAddImage = () => {
    if (newImageUrl.trim()) {
      setImages(prev => [newImageUrl.trim(), ...prev])
      setNewImageUrl('')
      setShowAddModal(false)
    }
  }

  const handleRemoveImage = (url) => {
    if (window.confirm('Are you sure you want to remove this image?')) {
      setImages(prev => prev.filter(img => img !== url))
    }
  }

  return (
    <div>

      <div className="container py-5">
        <h2 className="fw-bold mb-4 text-center"> Media Gallery</h2>
        <p className="text-muted text-center mb-5">A collection of memories and milestones captured from our past events.</p>

        {isAdmin && (
          <div className="text-end mb-4">
            <Button onClick={() => setShowAddModal(true)} className="admin-add-btn">
              ➕ Add New Image
            </Button>
          </div>
        )}

        <div className="row g-4">
          {images.map((src, i) => (
            <div key={i} className="col-sm-6 col-md-4 position-relative">
              <img
                src={src}
                alt={`Gallery ${i + 1}`}
                className="img-fluid rounded shadow-sm"
                style={{
                  cursor: 'pointer',
                  transition: 'transform 0.2s ease-in-out',
                  objectFit: 'cover',
                  height: '300px',
                }}
                onClick={() => handleImageClick(src)}
                onMouseOver={e => (e.currentTarget.style.transform = 'scale(1.05)')}
                onMouseOut={e => (e.currentTarget.style.transform = 'scale(1)')}
              />

              {isAdmin && (
                <button
                  className="image-delete-btn position-absolute top-0 end-0 m-2"
                  onClick={() => handleRemoveImage(src)}
                >
                  🗑️
                </button>
              )}
            </div>
          ))}
        </div>
      </div>

      {/* Fullscreen image viewer */}
      <Modal show={!!selectedImage} onHide={handleCloseModal} centered size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Full Image</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <img
            src={selectedImage}
            alt="Full Size"
            className="img-fluid w-100"
            style={{ objectFit: 'contain', maxHeight: '600px' }}
          />
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Add Image Modal */}
      <Modal show={showAddModal} onHide={() => setShowAddModal(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Add New Image</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Group>
            <Form.Label>Image URL</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter image URL"
              value={newImageUrl}
              onChange={(e) => setNewImageUrl(e.target.value)}
            />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowAddModal(false)}>
            Cancel
          </Button>
          <Button className="admin-add-btn" onClick={handleAddImage}>
            Add
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  )
}

export default MediaGalleryPage
