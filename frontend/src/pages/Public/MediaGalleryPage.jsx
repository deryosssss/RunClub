import React, { useState } from 'react'
import Header from '../../components/Header'  // Add this import
import { Modal, Button } from 'react-bootstrap'  // Add Bootstrap Modal

const images = [
  'https://images.unsplash.com/photo-1610816529690-1c56b5919f22?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
  'https://images.unsplash.com/photo-1731769698193-d4890f840d8d?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
  'https://images.unsplash.com/photo-1602070429746-fbef1c923e7f?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
  'https://plus.unsplash.com/premium_photo-1725905518200-b5a04d4faa90?q=80&w=3271&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
  'https://images.unsplash.com/photo-1719299246416-b4c069be9caf?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
  'https://images.unsplash.com/photo-1452626038306-9aae5e071dd3?q=80&w=3274&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
]

const MediaGalleryPage = () => {
  const [selectedImage, setSelectedImage] = useState(null)

  const handleImageClick = (src) => {
    setSelectedImage(src)
  }

  const handleCloseModal = () => {
    setSelectedImage(null)
  }

  return (
    <div>
      {/* Add the Header here */}
      <Header />

      <div className="container py-5">
        <h2 className="fw-bold mb-4 text-center">🖼️ Media Gallery</h2>
        <p className="text-muted text-center mb-5">A collection of memories and milestones captured from our past events.</p>

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
                onClick={() => handleImageClick(src)}  // Set the selected image on click
                onMouseOver={e => (e.currentTarget.style.transform = 'scale(1.05)')}
                onMouseOut={e => (e.currentTarget.style.transform = 'scale(1)')}
              />
              {/* Text Overlay that appears on hover */}
              <div className="image-overlay position-absolute w-100 h-100 d-flex align-items-center justify-content-center text-white">
                <p className="text-center mb-0 fw-bold">Gallery Image {i + 1}</p>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Modal for viewing full-sized image */}
      <Modal show={selectedImage !== null} onHide={handleCloseModal} centered size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Full Image</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <img
            src={selectedImage}
            alt="Full Size"
            className="img-fluid w-100"
            style={{ objectFit: 'contain', maxHeight: '600px' }}  // Adjust for better visibility
          />
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  )
}

export default MediaGalleryPage
